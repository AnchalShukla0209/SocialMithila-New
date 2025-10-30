$(document).ready(function () {
    $(".loader-overlay").css("display", "none");
})



function validateField(fieldId) {
    let value = $("#" + fieldId).val().trim();
    let errorField = $("#" + fieldId + "Error");
    let valid = true;

    errorField.text("");
    $("#" + fieldId).removeClass("error");

    switch (fieldId) {
        case "firstname":
            if (value === "") {
                errorField.text("First name is required");
                valid = false;
            } else if (!/^[a-zA-Z\s]+$/.test(value)) {
                errorField.text("Only alphabets allowed");
                valid = false;
            }
            break;

        case "emailid":
            if (value === "") {
                errorField.text("Email is required");
                valid = false;
            } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
                errorField.text("Invalid email format");
                valid = false;
            }
            break;

        case "oldpassword":
            if (value === "") {
                errorField.text("Password is required");
                valid = false;
            } else if (value.length < 6) {
                errorField.text("Password must be at least 6 characters");
                valid = false;
            }
            break;

        case "newpassword":
            let password = $("#oldpassword").val().trim();
            if (value === "") {
                errorField.text("Confirm your password");
                valid = false;
            } else if (value !== password) {
                errorField.text("Passwords do not match");
                valid = false;
            }
            break;
    }

    if (!valid) $("#" + fieldId).addClass("error");
    return valid;
}


$("#firstname, #emailid, #oldpassword, #newpassword").on("keyup blur", function () {
    validateField(this.id);
});


function validateForm() {
    let valid = true;
    ["firstname", "emailid", "oldpassword", "newpassword"].forEach(id => {
        if (!validateField(id)) valid = false;
    });
    return valid;
}

//$("#btnRegister").click(function () {
//    if (validateForm()) {
//        $(".loader-overlay").css("display", "flex");
//        let dto = {
//            FirstName: $("#firstname").val().trim(),
//            EmailId: $("#emailid").val().trim(),
//            OldPassword: $("#oldpassword").val().trim(),
//            NewPassword: $("#newpassword").val().trim()
//        };

//        $.ajax({
//            url: '/Auth/Registration',
//            type: 'POST',
//            data: JSON.stringify(dto),
//            contentType: 'application/json; charset=utf-8',
//            dataType: 'json',
//            success: function (res) {
//                if (res.success) {
//                    alert(res.msg);
//                    window.location.href = "/Auth/Login";
//                } else {
//                    alert(res.msg);
//                    $(".loader-overlay").css("display", "none");
//                }
//            },
//            error: function (err) {
//                console.error(err);
//                alert("Something went wrong.");
//                $(".loader-overlay").css("display", "none");
//            }
//        });
//    }
//});
$("#btnRegister").click(function () {
    if (validateForm()) {
        $(".loader-overlay").css("display", "flex");

        let dto = {
            FirstName: $("#firstname").val().trim(),
            EmailId: $("#emailid").val().trim(),
            OldPassword: $("#oldpassword").val().trim(),
            NewPassword: $("#newpassword").val().trim()
        };

        $.ajax({
            url: '/Auth/Registration',
            type: 'POST',
            data: JSON.stringify(dto),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (res) {
                $(".loader-overlay").css("display", "none");

                if (res.success) {
                    Swal.fire({
                        icon: "success",
                        title: "Registration Successful!",
                        text: res.msg,
                        showConfirmButton: false,
                        timer: 2000
                    }).then(() => {
                        window.location.href = "/Auth/Login";
                    });
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Registration Failed",
                        text: res.msg,
                        confirmButtonColor: "#d33"
                    });
                }
            },
            error: function (err) {
                console.error(err);
                $(".loader-overlay").css("display", "none");
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: "Something went wrong. Please try again later.",
                    confirmButtonColor: "#d33"
                });
            }
        });
    }
});
