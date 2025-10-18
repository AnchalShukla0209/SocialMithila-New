
$(document).ready(function () {

    $('#addReviewModal').modal({
        backdrop: false,
        keyboard: true,
        show: false
    });


    $('#addBusinessModal').modal({
        backdrop: false,
        keyboard: true,
        show: false
    });
    const maxAmenities = 20;

    $('#addAmenity').click(function () {
        const count = $('#amenitiesWrapper .amenity-item').length;
        if (count >= maxAmenities) {
            alert("You can add up to 20 amenities only.");
            return;
        }

        const newAmenity = `
            <div class="input-group mb-2 amenity-item">
                <input type="text" name="Aminities[]" class="form-control" placeholder="Enter amenity" required>
                <button type="button" class="btn btn-danger removeAmenity">&times;</button>
            </div>
        `;
        $('#amenitiesWrapper').append(newAmenity);

        // Enable all remove buttons except when only one remains
        updateAmenityButtons();
    });

    // Remove amenity input
    $(document).on('click', '.removeAmenity', function () {
        $(this).closest('.amenity-item').remove();
        updateAmenityButtons();
    });

    // Ensure at least one amenity field remains
    function updateAmenityButtons() {
        const total = $('#amenitiesWrapper .amenity-item').length;
        if (total <= 1) {
            $('.removeAmenity').prop('disabled', true);
        } else {
            $('.removeAmenity').prop('disabled', false);
        }
    }

    updateAmenityButtons();


    loadBusinesses(1);

    $('.product-slider').owlCarousel({
        loop: true,
        margin: 10,
        nav: true,
        dots: false,
        responsive: {
            0: { items: 1 },
            600: { items: 2 },
            1000: { items: 3 }
        }
    });


        // ==============================
        // 🔹 Load SubCategories dynamically
        // ==============================
        $('#CategoryId').change(function () {
            var categoryId = $(this).val();
            $.getJSON('/Business/GetSubCategories', { categoryId: categoryId }, function (data) {
                var subCat = $('#SubCategoryId');
                subCat.empty().append('<option value="">Select SubCategory</option>');
                $.each(data, function (i, item) {
                    subCat.append('<option value="' + item.SubCategoryId + '">' + item.SubCategoryName + '</option>');
                });
            });
        });

    // ==============================
    // 🔹 Image Preview + Validation
    // ==============================
        let selectedFiles = []; // store validated File objects

        $('#BusinessImages').on('change', function (e) {
            $('#previewContainer2').empty();
            selectedFiles = [];

            let files = Array.from(e.target.files);

            if (files.length > 5) {
                alert("You can upload a maximum of 5 images.");
                this.value = "";
                return;
            }

            let processed = 0;

            files.forEach((file, index) => {
                const reader = new FileReader();
                const img = new Image();

                reader.onload = function (event) {
                    img.onload = function () {
                        if (img.width !== 480 || img.height !== 320) {
                            alert(`Image ${file.name} must be 480x320 pixels.`);
                        } else {
                            selectedFiles.push(file);
                        }

                        processed++;
                        if (processed === files.length) updatePreview();
                    };
                    img.src = event.target.result;
                };

                reader.readAsDataURL(file);
            });
        });

        function updatePreview() {
            $('#previewContainer2').empty();

            selectedFiles.forEach((file, index) => {
                const reader = new FileReader();
                reader.onload = function (e) {
                    const $div = $(`
                <div class="position-relative me-2 mb-2 d-inline-block">
                    <img src="${e.target.result}" class="img-thumbnail" style="height:100px;width:100px;border-radius:10px;">
                    <button type="button" class="btn btn-sm btn-danger position-absolute top-0 end-0"
                            style="border-radius:50%;padding:2px 6px;">&times;</button>
                </div>`);

                    // Bind click handler dynamically
                    $div.find('button').on('click', function () {
                        selectedFiles.splice(index, 1);
                        updatePreview();
                        if (selectedFiles.length === 0) $('#BusinessImages').val('');
                    });

                    $('#previewContainer2').append($div);
                };
                reader.readAsDataURL(file);
            });
        }


        function removeImage(index) {
            selectedFiles.splice(index, 1);
            updatePreview();
            if (selectedFiles.length === 0) $('#BusinessImages').val('');
        }

        //$('#saveBusiness').off('click').on('click', function (e) {
        //    e.preventDefault();

        //    // Prevent double clicks
        //    if ($(this).data('submitting')) return;
        //    $(this).data('submitting', true);

        //    // Force manual submit event once
        //    $('#businessForm').trigger('submit');
        //});

    $(document).on('click', '#saveBusiness', function (e) {
        e.preventDefault();

        // prevent multiple clicks
        if ($(this).data('submitting')) return;
        $(this).data('submitting', true);

        if (selectedFiles.length === 0) {
            alert("Please upload at least one image (480x320).");
            $(this).data('submitting', false);
            return;
        }

        const form = $('#businessForm')[0];
        const formData = new FormData(form);

        // Remove any auto-included file inputs
        formData.delete("BusinessImages");

        // Append validated files
        selectedFiles.forEach(f => formData.append("BusinessImages", f));

        // 🌍 Add Latitude & Longitude
        if (window.currentLocation) {
            formData.append("Latitude", window.currentLocation.lat);
            formData.append("Longitude", window.currentLocation.lng);
        }

        // 🧾 Add Amenities
        $('#amenitiesWrapper input[name="Aminities[]"]').each(function () {
            const val = $(this).val().trim();
            if (val) formData.append("Aminities[]", val);
        });

        $.ajax({
            url: '/Business/AddBusiness',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (res) {
                $('#saveBusiness').data('submitting', false);
                if (res.success) {
                    alert(res.message);
                    $('#addBusinessModal').modal('hide');
                    $('#businessForm')[0].reset();
                    $('#previewContainer2').empty();
                    selectedFiles = [];
                    loadBusinesses(1);
                } else {
                    alert(res.message);
                }
            },
            error: function () {
                $('#saveBusiness').data('submitting', false);
                alert("Error while saving business.");
            }
        });
    });


    // 🌍 Detect location on page load
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            function (position) {
                window.currentLocation = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                console.log("📍Location detected:", window.currentLocation);
            },
            function (error) {
                console.warn("⚠️ Location not allowed:", error.message);
            }
        );
    }


}); 



function loadBusinesses(page = 1) {
    const sortBy = $('#sortBy').val();
    const search = $('#miniSearch').val();

    $.ajax({
        url: '/Business/GetAllBusinesses',
        data: { page, sortBy, search },
        success: function (html) {
            $('#listingsContainer').html(html);
        },
        error: function () {
            alert('Error loading businesses.');
        }
    });
}

$(document).on('click', '.btn-details', function (e) {
    e.preventDefault();

    const id = $(this).data('id');
    if (!id) {
        alert("Invalid business ID.");
        return;
    }

    const url = `/Business/BusinessDetails/${id}`;
    window.open(url, '_blank'); // open in new tab
});


$(document).on('change', '#sortBy', function () { loadBusinesses(1); });
$(document).on('keyup', '#miniSearch', function () { loadBusinesses(1); });
$(document).on('click', '.pagination a.page-link', function (e) {
    e.preventDefault();
    const page = $(this).data('page');
    if (page) loadBusinesses(page);
});

$(document).on('click', '.btn-follow', function () {
    const btn = $(this);
    const businessId = btn.data('id');

    $.ajax({
        url: '/Business/ToggleFollow',
        type: 'POST',
        data: { id: businessId },
        success: function (res) {
            if (res.success) {
                // Toggle follow/unfollow look
                const isFollowing = res.isFollowing;
                btn.toggleClass('btn-outline-secondary bg-primary-gradiant', true);
                btn.text(isFollowing ? 'Following' : 'Follow');
            } else {
                alert(res.message || "Unable to update follow status.");
            }
        },
        error: function () {
            alert("Something went wrong while following business.");
        }
    });
});

let selectedRating = 0;

$(document).on('click', '#starRating .star', function () {
    selectedRating = $(this).data('value');
    $('#starRating .star').each(function () {
        const starVal = $(this).data('value');
        $(this).toggleClass('text-warning', starVal <= selectedRating)
            .toggleClass('text-grey-400', starVal > selectedRating);
    });
});


$('#reviewForm').on('submit', function (e) {
    e.preventDefault();

    const data = {
        BusinessId: $('#BusinessId').val(),
        UserName: $('#UserName').val(),
        ReviewText: $('#ReviewText').val(),
        Rating: selectedRating
    };

    if (selectedRating === 0) {
        alert('Please select a rating.');
        return;
    }

    $.ajax({
        url: '/Business/AddReview',
        type: 'POST',
        data: data,
        success: function (response) {
            if (response.success) {
                $('#addReviewModal').modal('hide');
                $('#reviewForm')[0].reset();
                selectedRating = 0;
                $('#starRating .star').removeClass('text-warning').addClass('text-grey-400');

                // ✅ Update average rating and total ratings
                $('.averagerating').text(parseFloat(response.data.AverageRating).toFixed(2));
                $('.totalrating').text(`Based on ${response.data.TotalRatings}`);

                // ✅ Append the new review dynamically
                $('#reviewsContainer').prepend(`
                    <div class="row review-item mt-3">
                        <div class="col-2 text-left">
                            <figure class="avatar float-left mb-0">
                                <img src="${response.data.ProfilePhoto}" alt="${response.data.UserName}" class="float-right shadow-none w40 me-2 rounded-circle">
                            </figure>
                        </div>
                        <div class="col-10 ps-4">
                            <div class="content">
                                <h6 class="author-name font-xssss fw-600 mb-0 text-grey-800">${response.data.UserName}</h6>
                                <h6 class="d-block font-xsssss fw-500 text-grey-500 mt-2 mb-0">${response.data.CreatedAt}</h6>
                                <div class="star d-block w-100 text-left">${[...Array(5)].map((_, i) => `<img src="/asset/images/${i < response.data.Rating ? 'star' : 'star-disable'}.png" class="w10">`).join('')}
                                </div>
                                <p class="comment-text lh-24 fw-500 font-xssss text-grey-500 mt-2">${response.data.ReviewText}</p>
                            </div>
                        </div>
                    </div>
                `);
            } else {
                alert(response.message || 'Something went wrong!');
            }
        },
        error: function () {
            alert('Error submitting review. Please try again.');
        }
    });
});

$('#contactForm').on('submit', function (e) {
    e.preventDefault();

    const name = $('#contactName').val().trim();
    const email = $('#contactEmail').val().trim();
    const mobile = $('#contactMobile').val().trim();
    const message = $('#contactMessage').val().trim();
    const businessId = $('#BusinessId').val();

    // 🧩 Client-side validation
    if (!name || !email || !mobile || !message) {
        $('#contactMsg').text('All fields are required!').css('color', 'red');
        return;
    }
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        $('#contactMsg').text('Please enter a valid email address!').css('color', 'red');
        return;
    }

    $.ajax({
        url: '/Business/AddContact',
        type: 'POST',
        data: {
            BusinessId: businessId,
            Name: name,
            Email: email,
            MobileNumber: mobile,
            Message: message
        },
        success: function (res) {
            if (res.success) {
                $('#contactMsg').text(res.message).css('color', 'green');
                $('#contactForm')[0].reset();
            } else {
                $('#contactMsg').text(res.message).css('color', 'red');
            }
        },
        error: function () {
            $('#contactMsg').text('Something went wrong. Please try again.').css('color', 'red');
        }
    });
});


