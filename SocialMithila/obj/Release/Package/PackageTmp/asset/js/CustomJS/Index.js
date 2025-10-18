$(document).ready(function () {

   

    $('#Modalstory').modal({
        backdrop: false,
        keyboard: true,
        show: false
    });

    $('#cameraModal').modal({
        backdrop: false,
        keyboard: true,
        show: false
    });

    $('#previewModal').modal({
        backdrop: false,
        keyboard: true,
        show: false
    });

    $('#addBusinessModal').modal({
        backdrop: false,
        keyboard: true,
        show: false
    });



    //if (navigator.geolocation) {
    //    navigator.geolocation.getCurrentPosition(function (position) {
    //        var lat = position.coords.latitude;
    //        var lng = position.coords.longitude;

    //        var mapIframe = `<iframe
    //            width="100%"
    //            height="200"
    //            frameborder="0"
    //            style="border:0"
    //            src="https://www.google.com/maps?q=${lat},${lng}&hl=es;z=14&output=embed"
    //            allowfullscreen>
    //        </iframe>`;

    //        $('.map-placeholder').html(mapIframe);
    //    }, function (error) {
    //        $('.map-placeholder').html('<p class="text-center text-danger small">Unable to get location.</p>');
    //    });
    //} else {
    //    $('.map-placeholder').html('<p class="text-center text-danger small">Geolocation is not supported by your browser.</p>');
    //}
    $('.map-placeholder').html('<p class="text-center text-danger small">We are working on it....</p>')
    let pressTimer;

    //function pauseStoryStart(e) {
    //    e.preventDefault(); // prevent text selection or scrolling
    //    pauseStory();
    //}

    //function resumeStoryEnd(e) {
    //    e.preventDefault();
    //    resumeStory();
    //}

    //// Desktop
    //$("#storyWrapper").on("mousedown", pauseStoryStart);
    //$("#storyWrapper").on("mouseup mouseleave", resumeStoryEnd);

    //// Mobile
    //$("#storyWrapper").on("touchstart", pauseStoryStart);
    //$("#storyWrapper").on("touchend touchcancel", resumeStoryEnd);

    //$(document).on("focus", ".story-footer input", function () {
    //    pauseStory();
    //});

    //// When textbox loses focus → resume story
    //$(document).on("blur", ".story-footer input", function () {
    //    resumeStory();
    //});



    //function loadUserStories(userId, userIndex) {
    //    $.ajax({
    //        url: `/Home/GetStoriesByUser`,
    //        type: "GET",
    //        data: { userId: userId },
    //        success: function (data) {
    //            if (!data || data.length === 0) return;

    //            // stories for this user
    //            stories = data.map(story => ({
    //                id: story.StoryId,
    //                type: story.MediaType.toLowerCase(),
    //                src: story.MediaUrl,
    //                userId: story.UserId,
    //                isLiked: story.IsLikedByCurrentUser
    //            }));

    //            // set user info in header
    //            let userCard = $(userCards[userIndex]);
    //            let userName = userCard.data("user");
    //            let userPic = userCard.find("img").attr("src") || "https://cdn-icons-png.flaticon.com/512/149/149071.png";

    //            $("#storyUserName").text(userName);
    //            $("#storyUserPic").attr("src", userPic);

    //            currentIndex = 0;
    //            currentUserIndex = userIndex;

    //            renderProgressBars();
    //            $("#Modalstory").modal({
    //                backdrop: false,
    //                keyboard: true
    //            });

    //            const storyId = stories[currentIndex].id;
    //            $("#likeBtn").attr("data-storyid", storyId);
    //            const isLiked = stories[currentIndex].isLiked;
    //            const $likeBtn = $("#likeBtn");
    //            $likeBtn.attr("data-storyid", storyId);
    //            if (isLiked) {
    //                $likeBtn.addClass("liked bi-heart-fill").removeClass("bi-heart");
    //            } else {
    //                $likeBtn.removeClass("liked bi-heart-fill").addClass("bi-heart");
    //            }

    //            // mark as viewed
    //            if (stories[currentIndex].userId != userId) {
    //                $.post("/Home/MarkStoryViewed", { storyId });
    //            }

    //            // if current story belongs to logged-in user → show viewer panel
    //            if (stories[currentIndex].userId === userId) {
                    
    //                loadViewers(storyId);
    //            } 


    //            $("#Modalstory").modal("show");
    //            showStory(currentIndex);
    //        },
    //        error: function () {
    //            alert("Failed to load stories.");
    //        }
    //    });
    //}

    //function loadViewers(storyId) {
    //    $.ajax({
    //        url: "/Home/GetStoryViewers",
    //        type: "GET",
    //        data: { storyId },
    //        success: function (data) {
    //            const $list = $("#viewerList");
    //            $list.empty();
    //            $("#viewerCount").text(data.viewers.length);

    //            data.viewers.forEach(v => {
    //                const likedClass = v.IsLiked ? "bi-heart-fill text-danger viewer-heart" : "bi-heart viewer-heart";
    //                $list.append(`
    //                <li class="d-flex align-items-center mb-1">
    //                    <img src="${v.ProfilePhoto || 'https://cdn-icons-png.flaticon.com/512/149/149071.png'}" class="rounded-circle me-2">
    //                    <span class="viewer-name flex-grow-1">${v.Name}</span>
    //                    <i class="bi ${likedClass}"></i>
    //                </li>
    //            `);
    //            });

    //            // Show names and hearts after icon clicked
    //            setTimeout(() => $list.addClass("show-names"), 100);
    //        },
    //        error: function () {
    //            console.error("Failed to load viewers");
    //        }
    //    });
    //}



    //let stories = [];
    //let currentIndex = 0;
    //let timer;
    //let isPaused = false;
    //let remainingTime, startTime;

    //// Track users list (from carousel)
    //let userCards = [];
    //let currentUserIndex = 0;

    //function renderProgressBars() {
    //    $("#progressContainer").empty();
    //    stories.forEach((s, i) => {
    //        $("#progressContainer").append(`
    //        <div class="progress">
    //            <div class="progress-bar" id="progress-${i}"></div>
    //        </div>
    //    `);
    //    });
    //}

    //function showStory(index) {
    //    clearTimeout(timer);
    //    let story = stories[index];
    //    let storyID = story.id;
    //    const loggedInUserId = parseInt($("#hdnLogedinUserId").val());
    //    if (story.userId != loggedInUserId) {
    //        $.post("/Home/MarkStoryViewed", { storyID });
    //    }
       
    //    $("#storyImage, #storyVideo").addClass("d-none");
    //    $("#viewerList").empty(); // Reset viewers
    //    $("#viewersPanel").addClass("d-none"); // Hide panel
    //    $("#viewerCount").text("0");
    //    $("#viewerList").removeClass("show-names"); //

    //    if (story.type === "image") {
    //        $("#storyImage").attr("src", story.src).removeClass("d-none");
    //        startProgress(index, 5000); // image → 5s
    //    } else if (story.type === "video") {
    //        let video = $("#storyVideo").attr("src", story.src).removeClass("d-none")[0];
    //        video.currentTime = 0;
    //        video.play();
    //        video.onloadedmetadata = function () {
    //            startProgress(index, video.duration * 1000);
    //        };
    //    }

    //    const $likeBtn = $("#likeBtn");
    //    $likeBtn.attr("data-storyid", story.id);
    //    if (story.isLiked) {
    //        $likeBtn.addClass("liked bi-heart-fill").removeClass("bi-heart");
    //    } else {
    //        $likeBtn.removeClass("liked bi-heart-fill").addClass("bi-heart");
    //    }

    //    // =======================
    //    // Role-based UI toggle
    //    // =======================

        
    //    if (story.userId === loggedInUserId) {
    //        $("#commentLikeWrapper").addClass("d-none");
    //        $("#eyeWrapper").removeClass("d-none");
    //    } else {
    //        $("#commentLikeWrapper").removeClass("d-none");
    //        $("#eyeWrapper").addClass("d-none");
    //    }
    //}

    //// Click eye icon → toggle viewers panel + show names

    //// Click eye icon → toggle viewers panel + show names
    //const $tapZones = $(".tap-zone"); // already exists in your code

    //// Click eye icon → show viewers panel
    //$(document).on("click", "#storyViewersBtn", function () {
    //    const $panel = $("#viewersPanel");
    //    if ($panel.hasClass("d-none")) {
    //        pauseStory(); // pause story
    //        $panel.removeClass("d-none");
    //        $tapZones.css("pointer-events", "none"); // disable story tap while panel open
    //        loadViewers(stories[currentIndex].id);
    //    }
    //});

    //// Close button inside viewers panel
    //$(document).on("click", "#closeViewersPanel", function () {
    //    const $panel = $("#viewersPanel");
    //    $panel.addClass("d-none");
    //    $tapZones.css("pointer-events", "auto"); // re-enable story taps
    //    resumeStory(); // resume story
    //});

    

    //function startProgress(index, duration) {
    //    $(".progress-bar").stop().css("width", "0%");
    //    for (let i = 0; i < index; i++) $(`#progress-${i}`).css("width", "100%");

    //    startTime = Date.now();
    //    remainingTime = duration;

    //    $(`#progress-${index}`).animate({ width: "100%" }, duration, "linear", nextStory);
    //    timer = setTimeout(nextStory, duration);
    //}

    //function nextStory() {
    //    currentIndex++;
    //    if (currentIndex >= stories.length) {
    //        // finished this user's stories → go to next user
    //        nextUserStories();
    //    } else {
    //        showStory(currentIndex);
    //    }
    //}

    //function prevStory() {
    //    currentIndex--;
    //    if (currentIndex < 0) {
    //        // go to previous user if exists
    //        prevUserStories();
    //    } else {
    //        showStory(currentIndex);
    //    }
    //}

    //function pauseStory() {
    //    if (isPaused) return;
    //    isPaused = true;
    //    clearTimeout(timer);
    //    let elapsed = Date.now() - startTime;
    //    remainingTime -= elapsed;
    //    $(".progress-bar").stop();
    //    $("#storyVideo")[0].pause();
    //}

    //function resumeStory() {
    //    if (!isPaused) return;
    //    isPaused = false;
    //    startTime = Date.now();
    //    $(`#progress-${currentIndex}`).animate({ width: "100%" }, remainingTime, "linear", nextStory);
    //    timer = setTimeout(nextStory, remainingTime);
    //    if ($("#storyVideo").is(":visible")) {
    //        $("#storyVideo")[0].play();
    //    }
    //}


    //function nextUserStories() {
    //    currentUserIndex++;
    //    if (currentUserIndex >= userCards.length) {
    //        $("#Modalstory").modal("hide"); // no more users
    //    } else {
    //        let nextUserId = $(userCards[currentUserIndex]).data("userid");
    //        loadUserStories(nextUserId, currentUserIndex);
    //    }
    //}

    //function prevUserStories() {
    //    currentUserIndex--;
    //    if (currentUserIndex < 0) {
    //        currentUserIndex = 0;
    //        $("#Modalstory").modal("hide");
    //    } else {
    //        let prevUserId = $(userCards[currentUserIndex]).data("userid");
    //        loadUserStories(prevUserId, currentUserIndex);
    //    }
    //}

    //// ==========================
    //// Event bindings
    //// ==========================
    //$(document).on("click", "#tapLeft", prevStory);
    //$(document).on("click", "#tapRight", nextStory);

    ////$(document).on("click", "#storyWrapper", function () {
    ////    if (isPaused) resumeStory();
    ////    else pauseStory();
    ////});

    //$(document).on("click", "#customClose", function () {
    //    $('#Modalstory').modal('hide');
    //});

    //// Click story card → load that user
    //$(document).on("click", ".story-card", function () {
    //    userCards = $(".story-card"); // all story cards in carousel
    //    let userId = $(this).data("userid");
    //    let index = userCards.index(this);
    //    loadUserStories(userId, index);
    //});


    //// ============ LIKE BUTTON =============
    //$(document).on("click", "#likeBtn", function () {
    //    const storyId = stories[currentIndex].id; // include storyId in your array when loading
    //    const $icon = $(this);

    //    $.ajax({
    //        url: "/Home/StoryLike",
    //        type: "POST",
    //        data: { storyId },
    //        success: function (res) {
    //            if (res.success) {
    //                const liked = !$icon.hasClass("liked");
    //                $icon.toggleClass("liked bi-heart bi-heart-fill");


    //                // Floating heart animation
    //                if (liked) {
    //                    stories[currentIndex].isLiked = true;
    //                    const $heart = $('<i class="bi bi-heart-fill heart-float"></i>');
    //                    $(".story-container").append($heart);
    //                    setTimeout(() => $heart.remove(), 800);
    //                }
    //                else {
    //                    stories[currentIndex].isLiked = false;
    //                }
                   

    //                // Refresh viewers if story owner is viewing own story
    //                if (parseInt($("#hdnLogedinUserId").val()) === stories[currentIndex].userId) {
    //                    loadViewers(storyId);
    //                }
    //            }
    //        }
    //    });
    //});



    //function GetAllUserStories() {
    //    $.ajax({
    //        url: "/Home/GetAllUserStories",
    //        type: "GET",
    //        success: function (users) {
                
    //            let $carousel = $("#storiesCarousel");

    //            // clear carousel first
    //            $carousel.trigger("replace.owl.carousel", $("<div></div>"));
    //            $carousel.trigger("refresh.owl.carousel");


    //            // --- Logged-in user card ---
    //            let myUser = users.result.find(u => u.UserId === users.loggedInUserId);

    //            if (myUser) {
    //                // show "Your Story"
    //                let myStoryCard = `
    //            <div class="item">
    //                <div class="card story-card w125 h200 d-block border-0 shadow-none rounded-xxxl bg-dark overflow-hidden mb-3 mt-3"
    //                     data-userid="${myUser.UserId}" style="background-image:url(${myUser.MediaUrl});">
    //                    <div class="card-body d-block p-3 w-100 position-absolute bottom-0 text-center">
    //                        <a href="#">
    //                        <figure class="avatar ms-auto me-auto mb-0 position-relative w50 z-index-1">
    //                            <img src="${myUser.ProfilePic || "https://cdn-icons-png.flaticon.com/512/149/149071.png"}"
    //                                 class="float-right p-0 bg-white rounded-circle w-100 shadow-xss">
    //                         </figure>
    //                         <div class="clearfix"></div>
    //                         <h4 class="fw-600 text-white font-xssss mt-2 mb-1" id="addStoryBtn">Add Story</h4>
    //                        </a>
    //                    </div>
    //                </div>
    //            </div>`;
    //                $carousel.trigger("add.owl.carousel", [$(myStoryCard)]);
    //            } else {
    //                // show Add Story
    //                let addCard = `
    //            <div class="item">
    //                <div class="card w125 h200 d-block border-0 shadow-none rounded-xxxl bg-dark overflow-hidden mb-3 mt-3" id="addStoryBtn">
    //                    <div class="card-body d-block p-3 w-100 position-absolute bottom-0 text-center">
    //                        <a href="#">
    //                            <span class="btn-round-lg bg-white"><i class="feather-plus font-lg"></i></span>
    //                            <div class="clearfix"></div>
    //                            <h4 class="fw-700 position-relative z-index-1 ls-1 font-xssss text-white mt-2 mb-1">Add Story</h4>
    //                        </a>
    //                    </div>
    //                </div>
    //            </div>`;
    //                $carousel.trigger("add.owl.carousel", [$(addCard)]);
    //            }

    //            // --- Other users' stories ---
    //            users.result
    //                .filter(u => u.UserId !== users.loggedInUserId)
    //                .forEach(user => {
    //                    let cardHtml = `
    //                <div class="item">
    //                    <div class="card story-card w125 h200 d-block border-0 shadow-xss rounded-xxxl bg-gradiant-bottom overflow-hidden cursor-pointer mb-3 mt-3"
    //                         data-userid="${user.UserId}" style="background-image:url(${user.MediaUrl});">

    //                        ${user.MediaType === 'video' ? `
    //                        <video autoplay loop muted playsinline class="float-right w-100">
    //                            <source src="${user.MediaUrl}" type="video/mp4">
    //                        </video>` : `
    //                        `}

    //                        <div class="card-body d-block p-3 w-100 position-absolute bottom-0 text-center">
    //                            <a href="#">
    //                                <figure class="avatar ms-auto me-auto mb-0 position-relative w50 z-index-1">
    //                                    <img src="${user.ProfilePic || "https://cdn-icons-png.flaticon.com/512/149/149071.png"}"
    //                                         class="float-right p-0 bg-white rounded-circle w-100 shadow-xss">
    //                                </figure>
    //                                <div class="clearfix"></div>
    //                                <h4 class="fw-600 text-white font-xssss mt-2 mb-1">${user.UserName}</h4>
    //                            </a>
    //                        </div>
    //                    </div>
    //                </div>`;

    //                    $carousel.trigger("add.owl.carousel", [$(cardHtml)]);
    //                });

    //            $carousel.trigger("refresh.owl.carousel");
    //        }
    //    });
    //}



    //GetAllUserStories();


    //$(document).on("click", "#customClose", function () {
    //    $('#Modalstory').modal('hide');
    //});



    //let stream;
    //let capturedFile;
    //let cropper;

    //// Open camera when Add Story clicked
    //$(document).on("click", "#addStoryBtn", function (e) {
    //    e.stopPropagation(); // 🛑 Prevent click bubbling to .story-card

    //    navigator.mediaDevices.getUserMedia({ video: true })
    //        .then(s => {
    //            stream = s;
    //            $("#cameraStream")[0].srcObject = stream;
    //            $("#cameraModal").modal("show");

    //            $('#cameraModal').modal({
    //                backdrop: false,
    //                keyboard: true
    //            });
    //        })
    //        .catch(() => {
    //            $("#storyInput").click(); // fallback if no camera
    //        });
    //});



    //// Capture from camera
    //$("#captureBtn").on("click", function () {
    //    let video = $("#cameraStream")[0];
    //    let canvas = $("#captureCanvas")[0];
    //    let ctx = canvas.getContext("2d");

    //    canvas.width = video.videoWidth;
    //    canvas.height = video.videoHeight;
    //    ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

    //    let imageData = canvas.toDataURL("image/png");
    //    capturedFile = imageData;

    //    $("#cameraModal").modal("hide");
    //    showPreview("image", imageData);
    //});

    //// Upload file fallback
    //$("#uploadBtn").on("click", function () {
    //    $("#cameraModal").modal("hide");
    //    $("#storyInput").click();
    //});

    //// Handle file input
    //$("#storyInput").on("change", function (e) {
    //    let file = e.target.files[0];
    //    if (!file) return;

    //    if (file.type.startsWith("image/")) {
    //        let reader = new FileReader();
    //        reader.onload = function (evt) {
    //            capturedFile = evt.target.result;
    //            showPreview("image", capturedFile);
    //        };
    //        reader.readAsDataURL(file);
    //    } else if (file.type.startsWith("video/")) {
    //        let url = URL.createObjectURL(file);
    //        capturedFile = url;
    //        showPreview("video", capturedFile);
    //    }
    //});

    //// Show Preview (with Cropper for images)
    //function showPreview(type, src) {
    //    $("#previewContainer").empty();
    //    $("#cropImage").addClass("d-none");

    //    if (type === "image") {
    //        let img = $(`<img id="cropperImage" src="${src}" class="w-100">`);
    //        $("#previewContainer").append(img);
    //        $("#cropImage").removeClass("d-none");
    //        $("#previewModal").modal("show");

    //        $("#previewModal").one("shown.bs.modal", function () {
    //            if (cropper) cropper.destroy();
    //            cropper = new Cropper(document.getElementById("cropperImage"), {
    //                aspectRatio: 9 / 16,
    //                viewMode: 1,
    //                autoCropArea: 1,
    //                responsive: true,
    //                background: false,
    //            });
    //        });

    //        $("#previewModal").one("hidden.bs.modal", function () {
    //            if (cropper) {
    //                cropper.destroy();
    //                cropper = null;
    //            }
    //        });

    //    } else {
    //        $("#previewContainer").append(
    //            `<video src="${src}" controls autoplay playsinline class="w-100 rounded" style="max-height:80vh; object-fit:contain;"></video>`
    //        );
    //        $("#previewModal").modal("show");
    //    }
    //}

    //// Crop Button
    //$("#cropImage").on("click", function () {
    //    if (!cropper) return;

    //    let canvas = cropper.getCroppedCanvas({
    //        width: 720,
    //        height: 1280,
    //    });

    //    capturedFile = canvas.toDataURL("image/png");

    //    $("#previewContainer").html(`<img src="${capturedFile}" class="img-fluid rounded">`);

    //    cropper.destroy();
    //    cropper = null;

    //    $(this).addClass("d-none");
    //});

    //// Post Story
    //$("#postStory").on("click", function () {

    //    if (!capturedFile) {
    //        alert("No story to upload!");
    //        return;
    //    }

    //    // Get location first
    //    if (navigator.geolocation) {
    //        navigator.geolocation.getCurrentPosition(function (position) {
    //            uploadStory(position.coords.latitude, position.coords.longitude);
    //        }, function () {
    //            uploadStory(0, 0); // fallback if location denied
    //        });
    //    } else {
    //        uploadStory(0, 0); // fallback if geolocation not supported
    //    }
    //});

    //function uploadStory(lat, lng) {
    //    let formData = new FormData();

    //    if (capturedFile.startsWith("data:image")) {
    //        // Cropped or captured image (base64 → blob)
    //        let blob = dataURLtoBlob(capturedFile);
    //        formData.append("profilePhotoFile", blob, "story.png");
    //    } else if (capturedFile.startsWith("blob:")) {
    //        // Selected video from file input
    //        let videoFile = $("#storyInput")[0].files[0];
    //        formData.append("profilePhotoFile", videoFile);
    //    } else {
    //        // Direct file (image/video chosen)
    //        let file = $("#storyInput")[0].files[0];
    //        if (file) formData.append("profilePhotoFile", file);
    //    }

    //    formData.append("Latitude", lat);
    //    formData.append("Longitude", lng);

    //    $("#uploadProgressContainer").removeClass("d-none");
    //    $("#uploadProgressBar").css("width", "0%").text("0%");
    //    $(".loader-overlay").css("display", "flex");
    //    $.ajax({
    //        url: '/Home/AddStory',
    //        type: 'POST',
    //        data: formData,
    //        contentType: false,
    //        processData: false,
    //        xhr: function () {
    //            let xhr = new window.XMLHttpRequest();
    //            xhr.upload.addEventListener("progress", function (evt) {
    //                if (evt.lengthComputable) {
    //                    let percentComplete = Math.round((evt.loaded / evt.total) * 100);
    //                    $("#uploadProgressBar").css("width", percentComplete + "%").text(percentComplete + "%");
    //                }
    //            }, false);
    //            return xhr;
    //        },
    //        success: function (res) {
    //            if (res.success) {
    //                $("#uploadProgressBar").css("width", "100%").text("Uploaded!");
    //                setTimeout(() => {
    //                    $("#previewModal").modal("hide");
    //                    $("#uploadProgressContainer").addClass("d-none");
    //                    stopCameraStream();

    //                }, 1000);
    //                $(".loader-overlay").css("display", "none");
    //                alert("Story uploaded successfully!");
    //                GetAllUserStories();

    //            } else {
    //                $(".loader-overlay").css("display", "none");
    //                alert(res.msg);
    //            }
    //        },
    //        error: function (xhr, status, error) {
    //            $(".loader-overlay").css("display", "none");
    //            alert("Error: " + error);
    //        }
    //    });
    //}


    

    //// Helper: Convert base64 → Blob
    //function dataURLtoBlob(dataurl) {
    //    var arr = dataurl.split(','), mime = arr[0].match(/:(.*?);/)[1],
    //        bstr = atob(arr[1]), n = bstr.length, u8arr = new Uint8Array(n);
    //    while (n--) {
    //        u8arr[n] = bstr.charCodeAt(n);
    //    }
    //    return new Blob([u8arr], { type: mime });
    //}

    //// Stop camera when modal closes
    //$('#cameraModal').on('hidden.bs.modal', function () {
    //    stopCameraStream();
    //});

    //function stopCameraStream() {
    //    if (stream) {
    //        stream.getTracks().forEach(track => track.stop());
    //        stream = null;
    //    }
    //}




    function loadFriendRequests() {
        $(".loader-overlay").css("display", "flex");
        $.ajax({
            url: '/Home/GetFriendRequest',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.Success) {
                    var listContainer = $('#friendRequestList');
                    listContainer.empty(); // clear previous data

                    response.Data.forEach(function (friend) {
                        var fullName = (friend.FristNAme || '') + ' ' + (friend.LastNAme || '');
                        var profilePhoto = friend.ProfilePhoto && friend.ProfilePhoto.trim() !== ''
                            ? friend.ProfilePhoto
                            : 'https://cdn-icons-png.flaticon.com/512/149/149071.png'; // default image

                        var friendHtml = `
                        <div class="card-body d-flex pt-0 ps-4 pe-4 pb-0">
                            <figure class="avatar me-3">
                                <img src="${profilePhoto}" alt="image" class="shadow-sm rounded-circle w45">
                            </figure>
                            <h4 class="fw-700 text-grey-900 font-xssss mt-1">
                                ${fullName}
                                <span class="d-block font-xssss fw-500 mt-1 lh-3 text-grey-500">12 mutual friends</span>
                            </h4>
                        </div>
                        <div class="card-body d-flex align-items-center pt-0 ps-4 pe-4 pb-4">
                            <a href="javascript:void(0);"  data-friendshipid="${friend.FriendshipId}" data-requesterid="${friend.RequesterId}"  data-addressid="${friend.AddresseeId}"  class="p-2 lh-20 w100 bg-primary-gradiant me-2 text-white text-center font-xssss fw-600 ls-1 rounded-xl confirm-btn">Confirm</a>
                            <a href="javascript:void(0);" data-friendshipid="${friend.FriendshipId}"  data-requesterid="${friend.RequesterId}"  data-addressid="${friend.AddresseeId}"  class="p-2 lh-20 w100 bg-grey text-grey-800 text-center font-xssss fw-600 ls-1 rounded-xl delete-btn">Delete</a>
                        </div>`;

                        listContainer.append(friendHtml);
                        $(".loader-overlay").css("display", "none");
                    });

                    $('.confirm-btn').off('click').on('click', function () {
                        var friendshipId = $(this).data('friendshipid');
                        var requesterId = $(this).data('requesterid');
                        var addressid = $(this).data('addressid');
                        Confirm(friendshipId, addressid, $(this), requesterId);
                    });

                    $('.delete-btn').off('click').on('click', function () {
                        var friendshipId = $(this).data('friendshipid');
                        var requesterId = $(this).data('requesterid');
                        var addressid = $(this).data('addressid');
                        DeleteRequest(friendshipId, addressid, $(this), requesterId);
                    });

                } else {
                    $('#friendRequestList').html('<p class="text-center text-muted">No friend requests found.</p>');
                    $(".loader-overlay").css("display", "none");
                }
            },
            error: function (err) {
                console.error(err);
                $('#friendRequestList').html('<p class="text-center text-danger">Failed to load friend requests.</p>');
                $(".loader-overlay").css("display", "none");
            }
        });
    }




    loadFriendRequests();

});

function Confirm(friendshipId, addresseeId, buttonElement, requesterId) {
    const $btn = $(this);
    $.ajax({
        url: '/Home/AcceptFriendRequest',
        type: 'POST',
        data: { friendshipId: friendshipId, addresseeId: addresseeId, requesterId: requesterId },
        success: function (res) {
            if (res.success) {
                buttonElement.closest('.card-body').html('<p class="text-success">Friend request accepted.</p>');
                $btn.closest('.d-flex').html(`<span class="text-success font-xssss fw-600">${res.Message || 'Friend Request Accepted'}</span>`);
                loadFriends();
            } else {
                alert('Failed to accept friend request.');
            }
        },
        error: function (err) {
            console.error(err);
            alert('Server error occurred.');
        }
    });
}

function DeleteRequest(friendshipId, addresseeId, buttonElement, requesterId) {
    const $btn = $(this);
    $.ajax({
        url: '/Home/CancelFriendRequest',
        type: 'POST',
        data: { friendshipId: friendshipId, addresseeId: addresseeId, requesterId: requesterId },
        success: function (res) {
            if (res.success) {
                $btn.closest('.d-flex').html(`<span class="text-danger font-xssss fw-600">${res.Message || 'Friend Request Rejected'}</span>`);
                buttonElement.closest('.card-body').remove(); 
                loadFriends();
            } else {
                alert(res.message || 'Failed to delete friend request.');
            }
        },
        error: function (err) {
            console.error(err);
            alert('Server error occurred.');
        }
    });
}

var currentPage = 1;
var pageSize = 5;

function loadFriends(page = 1, search = '') {
    currentPage = page;
    $.get("/Home/GetFriends", { page: page, search: search, pageSize: pageSize }, function (res) {
        var html = '';
        $.each(res.data, function (i, friend) {
            html += `<div class="card-body bg-transparent-card d-flex p-3 bg-greylight ms-3 me-3 rounded-3 mb-2">
                            <figure class="avatar me-2 mb-0"><img src="${friend.ProfilePhotoUrl}" alt="image" class="shadow-sm rounded-circle w45"></figure>
                              <h4 class="fw-700 text-grey-900 font-xssss mt-2"> ${friend.FullName}<span class="d-block font-xssss fw-500 mt-1 lh-3 text-grey-500">${friend.MutualFriends} mutual friends</span></h4>
                               <a href="/User/UserProfile/${friend.UserId}" class="btn-round-sm bg-white text-grey-900 feather-chevron-right font-xss ms-auto mt-2"></a>
                               
                            </div>
                     `;
        });

        $('#friendsList').html(html);

        

        $('#paginationContainer').html(getPaginationHtml(res.page, res.totalPages));
    });
}

function getPaginationHtml(currentPage, totalPages) {
    let paginationHtml = '<nav><ul class="pagination pagination-sm mb-0" style="padding:10px">';

    // Previous button
    if (currentPage === 1)
        paginationHtml += '<li class="page-item disabled"><a class="page-link">Previous</a></li>';
    else
        paginationHtml += `<li class="page-item"><a class="page-link" href="#" onclick="loadFriends(${currentPage - 1},\'${$('#searchBox').val()}\')">Previous</a></li>`;

    // Page window
    let start = Math.max(1, currentPage - 2);
    let end = Math.min(totalPages, currentPage + 2);

    if (start > 1) {
        paginationHtml += `<li class="page-item"><a class="page-link" href="#" onclick="loadFriends(1,'${$('#searchBox').val()}')">1</a></li>`;
    }
    if (start > 2) {
        paginationHtml += '<li class="page-item disabled"><a class="page-link">...</a></li>';
    }

    for (let i = start; i <= end; i++) {
        if (i === currentPage)
            paginationHtml += `<li class="page-item active"><a class="page-link" href="#">${i}</a></li>`;
        else
            paginationHtml += `<li class="page-item"><a class="page-link" href="#" onclick="loadFriends(${i},\'${$('#searchBox').val()}\')">${i}</a></li>`;
    }

    if (end < totalPages - 1) {
        paginationHtml += '<li class="page-item disabled"><a class="page-link">...</a></li>';
    }
    if (end < totalPages) {
        paginationHtml += `<li class="page-item"><a class="page-link" href="#" onclick="loadFriends(${totalPages},\'${$('#searchBox').val()}\')">${totalPages}</a></li>`;
    }

    // Next button
    if (currentPage === totalPages)
        paginationHtml += '<li class="page-item disabled"><a class="page-link">Next</a></li>';
    else
        paginationHtml += `<li class="page-item"><a class="page-link" href="#" onclick="loadFriends(${currentPage + 1},\'${$('#searchBox').val()}\')">Next</a></li>`;

    paginationHtml += '</ul></nav>';
    return paginationHtml;
}


$(document).ready(function () {

    // Open Modal
    $("#btnAddBusiness").click(function () {
        $('#addBusinessModal').modal({
            backdrop: false,
            keyboard: true,
            show: false
        });

        $("#addBusinessModal").modal("show");
    });

    // Submit Form
    $("#businessForm").on("submit", function (e) {
        e.preventDefault();

        $.ajax({
            url: '/Business/AddBusiness',
            type: 'POST',
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    $("#addBusinessModal").modal("hide");
                    $("#businessForm")[0].reset();
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert("Something went wrong.");
            }
        });
    });

    loadFriends();

    $('#searchBox').on('keyup', function () {
        loadFriends(1, $(this).val());
    });
});