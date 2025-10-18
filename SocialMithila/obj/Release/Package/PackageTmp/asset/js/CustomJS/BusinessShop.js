
$(document).ready(function () {

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