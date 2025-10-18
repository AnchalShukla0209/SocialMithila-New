// Scripts/createPost.js
$(function () {
    var userId = parseInt($("#createUserId").val() || "0");
    var mentionBuffer = [];
    var filesToUpload = [];

    // --- Mentions ---
    $("#postContent").on("keyup", function () {
        
        var val = $(this).val();
        var caretPos = this.selectionStart;
        var sub = val.substring(0, caretPos);
        var atIndex = sub.lastIndexOf('@');
        if (atIndex >= 0) {
            var term = sub.substring(atIndex + 1);
            if (term.length >= 1 && !/\s/.test(term)) {
                fetchUsers(term);
            } else {
                $("#mentionSuggestions").hide();
            }
        } else {
            $("#mentionSuggestions").hide();
        }
    });

    function fetchUsers(term) {
        $.getJSON('/User/SearchUsers', { q: term })
            .done(function (data) {
                var ul = $("#mentionSuggestions").empty().show();
                $.each(data, function (i, u) {
                    var li = $('<li class="list-group-item list-group-item-action d-flex align-items-center" style="cursor:pointer"></li>');
                    var img = $('<img width="34" height="34" class="rounded-circle me-2">')
                        .attr('src', u.ProfilePhoto || 'https://cdn-icons-png.flaticon.com/512/149/149071.png');

                    // if LastName is null, fallback to empty string
                    var fullName = u.FirstName + (u.LastName ? (" " + u.LastName) : "");

                    var name = $('<div>').text(fullName);
                    li.append(img).append(name).data('user', u);

                    li.on('click', function () {
                        insertMention($(this).data('user'));
                        $("#mentionSuggestions").hide();
                    });

                    ul.append(li);
                });
            });
    }


    function insertMention(user) {
        var ta = $("#postContent")[0];
        var caret = ta.selectionStart;
        var text = ta.value;
        var pre = text.substring(0, caret);
        var atIndex = pre.lastIndexOf('@');
        var post = text.substring(caret);
        var token = '[' + user.FirstName + (user.LastName ? (" " + user.LastName) : "") + ']';
        ta.value = text.substring(0, atIndex) + token + post;
        mentionBuffer.push(user.Id);
    }

    // --- File preview ---
    $("#mediaFiles").on("change", function () {
        filesToUpload = Array.from(this.files);
        renderPreview();
    });

    function renderPreview() {
        var container = $("#mediaPreview").empty();
        if (filesToUpload.length === 0) { $("#previewArea").hide(); return; }
        $("#previewArea").show();
        $.each(filesToUpload, function (idx, f) {
            var reader = new FileReader();
            var wrap = $('<div class="position-relative me-2 mb-2" style="width:120px"></div>');
            var removeBtn = $('<button type="button" class="btn btn-sm btn-danger position-absolute" style="right:0;top:0;">x</button>');
            removeBtn.on('click', function () { filesToUpload.splice(idx, 1); renderPreview(); });
            wrap.append(removeBtn);

            if (f.type.indexOf('image') === 0) {
                reader.onload = function (ev) {
                    wrap.append('<img class="img-fluid rounded" style="width:120px;height:80px;object-fit:cover;" src="' + ev.target.result + '"/>');
                };
                reader.readAsDataURL(f);
            } else if (f.type.indexOf('video') === 0) {
                reader.onload = function (ev) {
                    wrap.append('<video controls style="width:120px;height:80px;object-fit:cover;" src="' + ev.target.result + '"></video>');
                };
                reader.readAsDataURL(f);
            }
            container.append(wrap);
        });
    }

    // --- Submit Post ---
    $("#btnSubmitPost").on("click", function () {
        var content = $("#postContent").val();
        if (!content && filesToUpload.length === 0) {
            alert("Please add some content or file.");
            return;
        }
        var fd = new FormData();
        fd.append("userId", userId);
        fd.append("contentText", content);
        fd.append("mentions", JSON.stringify(mentionBuffer));
        $.each(filesToUpload, function (i, f) { fd.append("files", f); });

        $.ajax({
            url: '/User/UploadPost',
            type: 'POST',
            data: fd,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.success) {
                    $("#postContent").val('');
                    filesToUpload = [];
                    renderPreview();
                    mentionBuffer = [];
                    console.log("Posted:", res.post);
                } else {
                    alert("Failed to post");
                }
            },
            error: function () { alert("Error posting"); }
        });
    });
  
});
