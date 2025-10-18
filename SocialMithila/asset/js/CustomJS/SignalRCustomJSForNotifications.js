// Save jQuery reference before other scripts potentially overwrite $


$j(function () {
    
    var userId = '@Session["UserId"]';
    if (!userId) return; // skip if not logged in

    // --- Hubs ---
    var postHub = $j.connection.postHub;
    var feedHub = $j.connection.feedHub;
    var notifHub = $j.connection.notificationHub;
    var friendHub = $j.connection.friendRequestHub;

    // -----------------------------
    // --- Client Callbacks ---
    // -----------------------------

    // --- PostHub: Receive new posts ---
    postHub.client.receivePost = function (postDto) {
        console.log("Realtime Post:", postDto);

        var exists = $j("#feed-container > .post-item[data-postid='" + postDto.PostId + "']").length > 0;
        if (exists) return;

        if (postDto.CreatedOn && typeof postDto.CreatedOn === "string") {
            postDto.CreatedOn = new Date(postDto.CreatedOn);
        }

        $j.ajax({
            url: '/User/RenderPartial2',
            type: 'POST',
            data: JSON.stringify(postDto),
            contentType: 'application/json',
            success: function (html) {
                if ($j("#feed-container > .post-item[data-postid='" + postDto.PostId + "']").length === 0) {
                    $j("#feed-container").prepend(html);
                }
            },
            error: function (err) {
                console.error("❌ Error rendering real-time post:", err);
            }
        });
    };

    // --- FeedHub: Like count updates ---
    feedHub.client.updateLikeCount = function (postId, newCount) {
        var postCard = $j(`.post-item[data-postid='${postId}']`);
        if (postCard.length) postCard.find(".like-text").text(newCount + " Like");
    };

    // --- NotificationHub: receive notifications ---
    notifHub.client.receiveNotification = function (notifObj) {
        console.log("🔔 Notification received:", notifObj);

        var $badge = $j('#notifCount');
        var count = parseInt($badge.text()) || 0;
        $badge.text(count + 1).show();

        if (notifObj.Message) showToastNotification(notifObj.Message);
        refreshNotifications();
    };

    // --- FriendRequestHub: real-time friend requests ---
    friendHub.client.friendRequestUpdated = function () {
        console.log("👥 Friend request update received (real-time). Reloading...");
        if (typeof window.loadFriendRequests === 'function') {
            window.loadFriendRequests();
        }
    };

    // -----------------------------
    // --- Hub Start & Query String ---
    // -----------------------------
    $j.connection.hub.qs = { userId: userId };

    $j.connection.hub.start().done(function () {
        console.log("✅ SignalR connected for all hubs");

        // Optional initial data loads
        refreshNotifications();
        if (typeof loadFriendRequests === 'function') loadFriendRequests();
    }).fail(function (err) {
        console.error("❌ SignalR connection failed:", err);
    });

    // -----------------------------
    // --- Notification Helpers ---
    // -----------------------------
    $j(document).on('click', '#dropdownMenu3, #notifCount', function () {
        refreshNotifications();
        $j('#notifCount').hide();
    });

    async function refreshNotifications() {
        try {
            const res = await $j.get('/User/GetNotifications');
            const $list = $j('#dynamicNoti');
            $list.empty();

            if (!res || res.length === 0) {
                $list.append('<div class="text-center text-muted py-3">No notifications</div>');
                return;
            }

            res.forEach(n => {
                const profilePhoto = n.ProfilePhoto || 'https://cdn-icons-png.flaticon.com/512/149/149071.png';
                const readClass = n.IsRead ? 'opacity-50' : '';
                let messageText = n.Message;

                switch (n.NotificationType) {
                    case "FriendRequest":
                        messageText = `${n.FromUserName} sent you a friend request.`; break;
                    case "FriendAccept":
                        messageText = `${n.FromUserName} accepted your friend request.`; break;
                    case "FriendReject":
                        messageText = `${n.FromUserName} rejected your friend request.`; break;
                    case "PostLike":
                        messageText = `${n.FromUserName} liked your post.`; break;
                }

                const html = `
                        <div class="card bg-transparent-card w-100 border-0 ps-5 mb-3 ${readClass}">
                            <img src="${profilePhoto}" alt="user" class="w40 position-absolute left-0 rounded-circle">
                            <h5 class="font-xsss text-grey-900 mb-1 mt-0 fw-700 d-block">
                                ${n.FromUserName}
                                <span class="text-grey-400 font-xsssss fw-600 float-right mt-1">${n.CreatedOn}</span>
                            </h5>
                            <h6 class="text-grey-500 fw-500 font-xssss lh-4">${messageText}</h6>
                        </div>`;
                $list.append(html);
            });
        } catch (error) {
            console.error("⚠️ Failed to load notifications:", error);
            $j('#dynamicNoti').html('<div class="text-center text-danger py-3">Failed to load notifications</div>');
        }
    }

    function showToastNotification(msg) {
        const toast = $j(`<div class="toast align-items-center text-white bg-primary border-0 position-fixed bottom-0 end-0 m-3 shadow">
                <div class="d-flex">
                    <div class="toast-body fw-600">${msg}</div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                </div>
            </div>`);
        $j('body').append(toast);
        const bsToast = new bootstrap.Toast(toast[0], { delay: 4000 });
        bsToast.show();
        toast.on('hidden.bs.toast', () => toast.remove());
    }
});