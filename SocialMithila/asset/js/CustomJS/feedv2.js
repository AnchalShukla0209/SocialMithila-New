var skip = 0;
var take = 5;
var isLoading = false;
var hasMore = true;


function parseNetDate(netDate) {
    var timestamp = parseInt(netDate.replace(/\/Date\((\d+)\)\//, '$1'));
    return new Date(timestamp);
}

function formatDate(date) {
    var options = { year: 'numeric', month: 'short', day: '2-digit', hour: '2-digit', minute: '2-digit', hour12: true };
    return date.toLocaleString('en-US', options);
}

function loadFeeds() {
    if (isLoading || !hasMore) return;
    isLoading = true;
    $("#loading").show();
    const urlPath = window.location.pathname; // e.g. "/User/UserProfile/3" or "/User/UserProfile"
    const parts = urlPath.split('/').filter(Boolean); // removes empty items
    const lastPart = parts.pop();
    const id = /^\d+$/.test(lastPart) ? lastPart : 0; // if last part is a number use it, else 0
    console.log(id);

    $.ajax({
        url: '/User/LoadFeeds',
        type: "GET",
        data: { skip: skip, take: take, id: id },
        success: function (data) {
            if (data && data.length > 0) {
                $.each(data, function (i, post) {
                    post.CreatedOn = formatDate(parseNetDate(post.CreatedOn));
                    $.ajax({
                        url: '/User/RenderPartial',
                        type: "POST",
                        data: JSON.stringify(post),
                        contentType: "application/json",
                        success: function (html) {
                            $("#feed-container").append(html);
                        }
                    });
                });
                skip += take;
            } else {
                hasMore = false;
            }
        },
        complete: function () {
            $("#loading").hide();
            isLoading = false;
        }
    });
}

// Trigger on page load
$(document).ready(function () {
    loadFeeds();

    //$(document).on("mouseenter", ".reaction-container", function () {
    //    $(this).find("#fbReactionWrapper").addClass("show");
    //}).on("mouseleave", ".reaction-container", function () {
    //    $(this).find("#fbReactionWrapper").removeClass("show");
    //});

    //$(document).on("click touchstart", ".reaction-container", function (e) {
    //    e.stopPropagation(); // prevent closing immediately
    //    var wrapper = $(this).find("#fbReactionWrapper");

    //    if (!wrapper.hasClass("show")) {
    //        // hide others
    //        $(".reaction-container #fbReactionWrapper").removeClass("show");
    //        wrapper.addClass("show");
    //    } else {
    //        wrapper.removeClass("show");
    //    }
    //});




    //$(document).on("click", ".fb-reaction", function () {
    //    var reaction = $(this).data("reaction");
    //    var postId = $(this).closest(".post-item").data("postid");
    //    sendReaction(postId, reaction);
    //    $(this).closest("#fbReactionWrapper").removeClass("hide");
    //});



    $(document).on("mouseenter", ".reaction-container", function () {
        $(this).find("#fbReactionWrapper").addClass("show");
    }).on("mouseleave", ".reaction-container", function () {
        $(this).find("#fbReactionWrapper").removeClass("show");
    });

    // Mobile: click to toggle reaction wrapper
    $(document).on("click touchstart", ".reaction-container", function (e) {
        e.stopPropagation(); // prevent closing immediately
        var wrapper = $(this).find("#fbReactionWrapper");

        if (!wrapper.hasClass("show")) {
            $(".reaction-container #fbReactionWrapper").removeClass("show");
            wrapper.addClass("show");
        } else {
            wrapper.removeClass("show");
        }
    });

    // Click outside closes reaction wrapper
    $(document).on("click touchstart", function () {
        $(".reaction-container #fbReactionWrapper").removeClass("show");
    });

    // Reaction click (works on mobile & desktop)
    $(document).on("click touchstart", ".fb-reaction", function (e) {
        e.stopPropagation(); // prevent wrapper from closing
        var reaction = $(this).data("reaction");
        var postId = $(this).closest(".post-item").data("postid");

        sendReaction(postId, reaction);

        // Hide the reaction wrapper after selection
        $(this).closest("#fbReactionWrapper").removeClass("show");
    });


    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
            loadFeeds();
        }
    });
});

function sendReaction(postId, reactionType) {
    $.ajax({
        url: "/Home/LikePost",
        type: "POST",
        data: { postId: postId, reactionType: reactionType },
        success: function (res) {
            if (res.success) {

                var postCard = $(`.post-item[data-postid='${postId}']`);
                if (!postCard.length) return;
                var html = buildReactionSummary(res.data.UserReactions);
                postCard.find(".reaction-summary").html(html);

                switch (res.data.ReactionType) {
                    case "Love":
                        iconClass = '<img src="/asset/images/Icons/heart.svg" style="height: 28px;width: 28px" />  <span class="like-text4"> &nbsp;&nbsp; ' + res.data.ReactionType + ' </span>';
                        break;
                    case "Wow":
                        iconClass = '<img src="/asset/images/Icons/wow.svg" style="height: 28px;width: 28px" /> <span class="like-text4"> &nbsp;&nbsp; ' + res.data.ReactionType + ' </span>';
                        break;
                    case "Sad":
                        iconClass = '<img src="/asset/images/Icons/sad.svg" style="height: 28px;width: 28px" /> <span class="like-text4"> &nbsp;&nbsp; ' + res.data.ReactionType + ' </span>';
                        break;
                    case "Angry":
                        iconClass = '<img src="/asset/images/Icons/angry.svg" style="height: 28px;width: 28px" /> <span class="like-text4"> &nbsp;&nbsp; ' + res.data.ReactionType + ' </span>';
                        break;
                    case "Care":
                        iconClass = '<img src="/asset/images/Icons/osm.svg" style="height: 28px;width: 28px" /> <span class="like-text4"> &nbsp;&nbsp; ' + res.data.ReactionType + ' </span>';
                        break;
                    case "Laugh":
                        iconClass = '<img src="/asset/images/Icons/laugh.svg" style="height: 28px;width: 28px" /> <span class="like-text4"> &nbsp;&nbsp; ' + res.data.ReactionType + ' </span>';
                        break;
                    default:
                        iconClass = '<img src="/asset/images/Icons/like.svg" style="height: 28px;width: 28px" /> <span class="like-text4"> &nbsp;&nbsp; ' + res.data.ReactionType + ' </span>';
                        break;
                }
                postCard.find(".btn-like").html(iconClass);
            }
        },
        error: function (err) {
            console.log("Error:", err);
        }
    });
}


function buildReactionSummary(reactions) {
    if (!reactions || reactions.length === 0) return "";
    var icons = [...new Set(reactions.map(r => r.ReactionType))]
        .slice(0, 3)
        .map(r => {

            switch (r) {
                case "Love":
                    return icons = '<img src="/asset/images/Icons/heart.svg" style="height: 22px;width: 22px" />';

                case "Wow":
                    return icons = '<img src="/asset/images/Icons/wow.svg" style="height: 22px;width: 22px" />';

                case "Sad":
                    return icons = '<img src="/asset/images/Icons/sad.svg" style="height: 22px;width: 22px" />';

                case "Angry":
                    return icons = '<img src="/asset/images/Icons/angry.svg" style="height: 22px;width: 22px" /> ';

                case "Care":
                    return icons = '<img src="/asset/images/Icons/osm.svg" style="height: 22px;width: 22px" /> ';

                case "Laugh":
                    return icons = '<img src="/asset/images/Icons/laugh.svg" style="height: 22px;width: 22px" />';

                default:
                    return icons = '<img src="/asset/images/Icons/like.svg" style="height: 22px;width: 22px" /> ';

            }

        }).join('');

    var names = reactions.map(r => r.UserName);
    var text = `${names[0]}${names.length > 1 ? ` and ${names.length - 1} others` : ''}`;

    return `<div class="d-flex align-items-center">
                <span class="me-1">${icons}</span>
                <span class="fw-500 text-grey-900 font-xssss" title="${names.join(', ')}">${text}</span>
            </div>`;
}



let lastLoadedPostId = null;
let lightboxLoadTimer = null;

// Observe when Lightbox opens
const observer = new MutationObserver(function () {
    // Wait a short moment to let DOM stabilize (avoid multiple triggers)
    clearTimeout(lightboxLoadTimer);
    lightboxLoadTimer = setTimeout(function () {

        const $lightbox = $('#lightbox:visible');
        if ($lightbox.length) {
            const $currentImg = $lightbox.find('img.lb-image');

            if ($currentImg.length) {
                const imgSrc = $currentImg.attr('src');

                // Find the post containing this image
                const $post = $('.post-item').has(`img[src="${imgSrc}"]`);
                const postId = $post.data('postid');

                if ($.connection.commentHub && $.connection.commentHub.server) {
                    $.connection.commentHub.server.joinPostGroup(postId.toString())
                        .done(() => console.log(`✅ Joined SignalR group for post ${postId}`))
                        .fail(err => console.error("❌ Failed to join SignalR group:", err));
                }

                // Avoid repeated calls for same post
                if (postId && postId !== lastLoadedPostId) {
                    lastLoadedPostId = postId;
                    console.log('📦 Loading comments for post', postId);

                    $.ajax({
                        url: '/User/LoadComments',
                        type: 'GET',
                        data: { postId: postId },
                        success: function (html) {
                            // Remove any old comment panel and inject new
                            $lightbox.find('.right-comment').remove();
                            $lightbox.append(`
                                <div class="right-comment chat-left scroll-bar theme-dark-bg" style="padding-bottom: 0 !important">
                                    ${html}
                                </div>
                            `);
                        },
                        error: function () {
                            console.error('❌ Error loading comments.');
                        }
                    });
                }
            }
        } else {
            // Lightbox closed → reset
            lastLoadedPostId = null;
        }

    }, 300); // delay prevents rapid multiple triggers
});

// Observe DOM changes globally
observer.observe(document.body, { childList: true, subtree: true });


$(function () {
    const userId = $("#hdnuserId").val(); // logged-in user id
    const postId = $("#PostId").val(); // post Id



    // --- 2️⃣ Post a new comment ---
    $(document).on("click", "#btnPostComment", function () {
        const text = $("#txtNewComment").val().trim();
        if (!text) return;

        const postId = $("#PostId").val();
        $.ajax({
            url: '/User/AddComment',
            type: 'POST',
            data: {
                PostId: postId,
                UserId: userId,
                ParentCommentId: null,
                CommentText: text
            },
            success: function (res) {
                if (res.IsSuccess) {
                    console.log("✅ Comment added successfully");
                    $("#txtNewComment").val('');

                    // Auto-refresh this post's comments
                    let $container = $(".comment-popup:visible .right-comment");
                    if (!$container.length) {
                        const $lightbox = $("#lightbox:visible");
                        if ($lightbox.length) $container = $lightbox.find(".right-comment");
                    }

                    if ($container.length) {
                        console.log("♻️ Reloading comments (self-update)");
                        window.loadCommentsForPost(postId, $container);
                    }
                }
            },
            error: function () {
                console.error("❌ Error posting comment");
            }
        });
    });



    // --- 3️⃣ Show reply box ---
    $(document).on("click", ".reply-btn", function (e) {
        e.preventDefault();
        replyParentId = $(this).data("parent");
        replyUserName = $(this).data("username");

        $("#ParentCommentId").val(replyParentId);
        $("#txtReplyComment").val("@" + replyUserName + " ").focus();

        $(".reply-input-box").removeClass("d-none");
        $(".comment-input-box").addClass("d-none");

        $("html, body").animate({
            scrollTop: $(".reply-input-box").offset().top - 100
        }, 500);
    });

    $(document).on("click", ".like-comment-btn", function () {
        var $this = $(this);
        var commentId = $this.data("commentid");

        $.ajax({
            type: "POST",
            url: "/Home/LikeComment",
            data: { commentId: commentId },
            success: function (res) {
                if (res.success) {
                    let liked = $this.hasClass("liked");
                    if (liked) {
                        $this.removeClass("bi-heart-fill text-danger liked").addClass("bi-heart");

                    } else {
                        $this.removeClass("bi-heart").addClass("bi-heart-fill text-danger liked");
                    }

                    $this.closest(".like-container").find(".like-count-comment").text(res.data.data + "Like");
                }
            }
        });
    });

    $(document).on("click", "#btnCancelReply", function () {
        replyParentId = null;
        replyUserName = null;
        $("#ParentCommentId").val("");
        $("#txtReplyComment").val("");
        $(".reply-input-box").addClass("d-none");
        $(".comment-input-box").removeClass("d-none");
    });

    $(document).on("click", "#btnSendReply", function () {
        const postId = $("#PostId").val();
        const text = $("#txtReplyComment").val().trim();
        const parentId = $("#ParentCommentId").val();
        if (!text || !parentId) return;

        $.ajax({
            url: '/User/AddComment',
            type: 'POST',
            data: {
                PostId: postId,
                UserId: userId,
                ParentCommentId: parentId,
                CommentText: text
            },
            success: function (res) {
                if (res.IsSuccess) {
                    console.log("✅ Reply added successfully");
                    $("#txtReplyComment").val('');
                    $(".reply-input-box").addClass("d-none");
                    $("#ParentCommentId").val('');

                    // Auto-refresh comments for that post
                    let $container = $(".comment-popup:visible .right-comment");
                    if (!$container.length) {
                        const $lightbox = $("#lightbox:visible");
                        if ($lightbox.length) $container = $lightbox.find(".right-comment");
                    }

                    if ($container.length) {
                        console.log("♻️ Reloading comments (reply update)");
                        window.loadCommentsForPost(postId, $container);
                    }
                }
            },
            error: function () {
                console.error("❌ Error sending reply");
            }
        });
    });

    // --- 4️⃣ Send Reply ---
    $(document).on("click", ".btn-send-reply", function () {
        var postId = $("#PostId").val();
        const $replyBox = $(this).closest(".reply-box");
        const text = $replyBox.find(".reply-input").val().trim();
        if (!text) return;

        const parentId = $(this).closest("[data-commentid]").data("commentid");
        var postId = $("#PostId").val();
        $.ajax({
            url: '/User/AddComment',
            type: 'POST',
            data: {
                PostId: postId,
                UserId: userId,
                ParentCommentId: parentId,
                CommentText: text
            },
            success: function (res) {
                if (res.IsSuccess) {
                    console.log("✅ Reply added successfully");
                    $("#txtReplyComment").val('');
                    $(".reply-input-box").addClass("d-none");
                    $("#ParentCommentId").val('');

                    // Auto-refresh comments for that post
                    let $container = $(".comment-popup:visible .right-comment");
                    if (!$container.length) {
                        const $lightbox = $("#lightbox:visible");
                        if ($lightbox.length) $container = $lightbox.find(".right-comment");
                    }

                    if ($container.length) {
                        console.log("♻️ Reloading comments (reply update)");
                        window.loadCommentsForPost(postId, $container);
                    }
                }
            }
        });
    });


    $(document).on("click", ".post-reply", function () {
        const parentId = $(this).data("parentid");
        const text = $(this).closest(".reply-input-box").find(".reply-text").val().trim();
        var postId = $("#PostId").val();
        if (!text) return;

        $.ajax({
            url: "/User/AddComment",
            type: "POST",
            data: {
                PostId: postId,
                UserId: userId,
                ParentCommentId: parentId,
                CommentText: text
            },
            success: function (res) {
                if (res.IsSuccess) {
                    console.log("✅ Reply added successfully");
                    $("#txtReplyComment").val('');
                    $(".reply-input-box").addClass("d-none");
                    $("#ParentCommentId").val('');

                    // Auto-refresh comments for that post
                    let $container = $(".comment-popup:visible .right-comment");
                    if (!$container.length) {
                        const $lightbox = $("#lightbox:visible");
                        if ($lightbox.length) $container = $lightbox.find(".right-comment");
                    }

                    if ($container.length) {
                        console.log("♻️ Reloading comments (reply update)");
                        window.loadCommentsForPost(postId, $container);
                    }
                }
            },
            error: function (xhr) {
                console.error("❌ Reply error:", xhr.responseText);
            }
        });
    });


    // --- Start connection ---
    $.connection.hub.start();
});

// ---------------------
// Global state (only once)
// ---------------------
if (typeof window.commentManagerInitialized === 'undefined') {
    window.lastLoadedPostId = null;
    window.lightboxLoadTimer = null;
    window.commentManagerInitialized = true;
    window.currentSignalRGroup = null;   // Currently joined SignalR group
    window.isLightboxOpen = false;       // Track if lightbox is open
    window.lightboxLoadTimer = null;     // Debounce for MutationObserver

    // ---------------------
    // Remove existing comment container
    // ---------------------
    window.removeCommentContainer = function () {
        // Remove comment elements
        $('.comment-popup').remove();
        $('#lightbox .right-comment').remove();

        // Detect lightbox
        const $lightboxEl = $('#lightbox, .lightbox, .lb').first();
        const $overlayEl = $('#lightboxOverlay, .lightboxOverlay').first();

        // Decide if lightbox has images
        const hasImage = $lightboxEl.find('img.lb-image, img').length > 0;

        if (hasImage) {
            console.log('📷 Lightbox contains image(s) - closing sequence started.');

            // Remove Facebook-like Lightbox2 open state
            $('body').removeClass('lb-open');

            // Smooth fade out for overlay and lightbox
            $overlayEl.stop(true, true).fadeOut(200, function () {
                $(this).hide();
                console.log('🟢 Overlay closed.');
            });

            $lightboxEl.stop(true, true).fadeOut(250, function () {
                $(this)
                    .removeClass('show open active visible')
                    .attr('aria-hidden', 'true')
                    .hide();
                console.log('✅ Lightbox closed.');
            });

            $(document).off('click', '[data-lightbox]');
            console.log('🔒 Disabled Lightbox triggers temporarily.');

            // Re-enable after a short delay
            setTimeout(() => {
                if (window.lightbox && typeof window.lightbox.init === 'function') {
                    window.lightbox.init();
                    console.log('🔄 Lightbox reinitialized safely.');
                }
            }, 500);
        } else {
            window.isLightboxOpen = false; // observer will detect the lightbox opening
            $firstImg.trigger('click');
            setTimeout(function () {
                const $lightbox = $('#lightbox:visible');
                if ($lightbox.length) {
                    const alreadyHasComments = $lightbox.find('.right-comment').length > 0;
                    if (!alreadyHasComments) {
                        // find the image src and ensure we target the correct post
                        const imgSrc = $firstImg.find('img').attr('src') || $firstImg.attr('href') || $firstImg.data('src');
                        const $post = $('.post-item').has(`img[src="${imgSrc}"], a[data-lightbox][href="${imgSrc}"]`);
                        const postIdFromImg = $post.data('postid') || postId;

                        if (postIdFromImg) {
                            console.log('🔁 Fallback loading comments for post', postIdFromImg);
                            window.loadCommentsForPost(postIdFromImg, $lightbox);
                        }
                    }
                }
            }, 250);
            console.log('ℹ️ No image found inside lightbox — nothing to close.');
        }

        // --- SignalR cleanup (existing logic) ---
        if (window.currentSignalRGroup && $.connection?.commentHub?.server) {
            $.connection.commentHub.server.leavePostGroup(window.currentSignalRGroup.toString())
                .done(() => console.log(`🚪 Left SignalR group for post ${window.currentSignalRGroup}`))
                .fail(err => console.error("❌ Failed to leave SignalR group:", err));
        }

        // Reset state
        window.currentSignalRGroup = null;
        window.isLightboxOpen = false;
    };

    // ---------------------
    // Load comments
    // ---------------------
    window.loadCommentsForPost = function (postId, $container) {
        if (!postId) return console.warn('❌ No postId provided');

        // Leave previous group if different
        if (window.currentSignalRGroup && window.currentSignalRGroup !== postId) {
            if ($.connection.commentHub && $.connection.commentHub.server) {
                $.connection.commentHub.server.leavePostGroup(window.currentSignalRGroup.toString())
                    .done(() => console.log(`🚪 Left SignalR group for post ${window.currentSignalRGroup}`))
                    .fail(err => console.error("❌ Failed to leave SignalR group:", err));
            }
        }

        // Update current group
        window.currentSignalRGroup = postId;

        // Join SignalR group
        if ($.connection.commentHub && $.connection.commentHub.server) {
            $.connection.commentHub.server.joinPostGroup(postId.toString())
                .done(() => console.log(`✅ Joined SignalR group for post ${postId}`))
                .fail(err => console.error("❌ Failed to join SignalR group:", err));
        }

        // Fetch comments via AJAX
        $.ajax({
            url: '/User/LoadComments',
            type: 'GET',
            data: { postId: postId },
            success: function (html) {
                $container.find('.right-comment').remove();
                $container.append(`
                <div class="right-comment chat-left scroll-bar theme-dark-bg" style="padding-bottom: 0 !important">
                    ${html}
                </div>
            `);
            },
            error: function () {
                console.error('❌ Error loading comments.');
            }
        });
    };

    // ---------------------
    // Lightbox observer (detect open/close)
    // ---------------------
    window.lightboxObserver = new MutationObserver(function () {
        clearTimeout(window.lightboxLoadTimer);
        window.lightboxLoadTimer = setTimeout(function () {
            const $lightbox = $('#lightbox:visible');

            if ($lightbox.length) {
                if (!window.isLightboxOpen) {
                    window.isLightboxOpen = true;

                    const $currentImg = $lightbox.find('img.lb-image');
                    if ($currentImg.length) {
                        const imgSrc = $currentImg.attr('src');
                        const $post = $('.post-item').has(`img[src="${imgSrc}"]`);
                        const postId = $post.data('postid');

                        console.log('🖼️ Lightbox opened with image:', imgSrc);
                        window.loadCommentsForPost(postId, $lightbox);
                    }
                }
            } else {
                if (window.isLightboxOpen) {
                    window.isLightboxOpen = false;

                    if (window.currentSignalRGroup && $.connection.commentHub && $.connection.commentHub.server) {
                        $.connection.commentHub.server.leavePostGroup(window.currentSignalRGroup.toString())
                            .done(() => console.log(`🚪 Left SignalR group for post ${window.currentSignalRGroup} (lightbox closed)`))
                            .fail(err => console.error("❌ Failed to leave SignalR group:", err));
                        window.currentSignalRGroup = null;
                    }
                }
            }
        }, 300);
    });
    window.lightboxObserver.observe(document.body, { childList: true, subtree: true });



    // ---------------------
    // Comment icon click → open lightbox or popup
    // ---------------------
    $(document).on('click', '.comment-icon', function (e) {
        e.preventDefault();

        const $post = $(this).closest('.post-item');
        const postId = $post.data('postid');

        console.log('💬 Comment icon clicked for postId:', postId);

        const $firstImg = $post.find('a[data-lightbox]').first();

        if (!$firstImg.length) {
            // Post without image → popup
            window.removeCommentContainer(); // safe to remove old group

            const $popup = $(`
            <div class="comment-popup slide-up">
                
                <div class="right-comment chat-left scroll-bar theme-dark-bg" style="padding-bottom: 0 !important"></div>
            </div>
        `);
            $('body').append($popup);
            void $popup[0].offsetWidth; // trigger reflow
            $popup.addClass('visible');

            const $commentContainer = $popup.find('.right-comment');
            window.loadCommentsForPost(postId, $commentContainer);

        } else {
            // Post with image → lightbox
            // Do NOT removeCommentContainer, observer will handle joining/leaving
            window.isLightboxOpen = false; // observer will detect the lightbox opening
            $firstImg.trigger('click');
        }
    });



    // ---------------------
    // Close popup
    // ---------------------
    $(document).on('click', '.close-comment-popup', function () {
        window.removeCommentContainer();
    });
}

$(document).on('click', '.close-comment-popup', function () {
    window.removeCommentContainer();
});





