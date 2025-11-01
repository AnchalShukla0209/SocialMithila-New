$(document).ready(function () {

    $(".loader-overlay").css("display", "none");

    function validateLogin() {
        let email = $("#emailid").val().trim();
        let password = $("#password").val().trim();
        let valid = true;

        $("#emailError").text("");
        $("#passwordError").text("");


        if (email === "") {
            $("#emailError").text("Email is required");
            valid = false;
        } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
            $("#emailError").text("Invalid email format");
            valid = false;
        }

        if (password === "") {
            $("#passwordError").text("Password is required");
            valid = false;
        } else if (password.length < 6) {
            $("#passwordError").text("Password must be at least 6 characters");
            valid = false;
        }

        return valid;
    }



    $("#emailid, #password").on("keyup blur", function () {
        validateLogin();
    });
    $("#OTP").on("keyup blur", function () {
        validateOtp();
    });

    function Proceed() {
        window.location.href = "/Home/Index";
    }

    //$("#btnLogin").click(function () {
    //    if (validateLogin()) {
    //        $(".loader-overlay").css("display", "flex");
    //        let email = $("#emailid").val().trim();
    //        let password = $("#password").val().trim();

    //        // Client-side validation
    //        if (email === "") {
    //            alert("Please enter Email Id");
    //            $("#emailid").focus();
    //            return;
    //        }
    //        if (password === "") {
    //            alert("Please enter Password");
    //            $("#password").focus();
    //            return;
    //        }

    //        // Prepare request model
    //        let loginDTO = {
    //            EmailId: email,
    //            Password: password
    //        };

    //        $.ajax({
    //            url: '/Auth/Login',
    //            type: 'POST',
    //            data: JSON.stringify(loginDTO),
    //            contentType: 'application/json; charset=utf-8',
    //            dataType: 'json',
    //            success: function (res) {
    //                if (res.success) {
    //                    alert(res.msg);
    //                    $("#otpphase").css("display", "block");
    //                    $("#login1").css("display", "none");
    //                    $(".loader-overlay").css("display", "none");
    //                } else {
    //                    alert(res.msg);
    //                    $(".loader-overlay").css("display", "none");
    //                }
    //            },
    //            error: function (err) {
    //                console.error(err);
    //                alert("Something went wrong. Please try again later.");
    //                $(".loader-overlay").css("display", "none");
    //            }
    //        });
    //    }
    //});

    $("#btnLogin").click(function () {
        if (validateLogin()) {
            $(".loader-overlay").css("display", "flex");

            let email = $("#emailid").val().trim();
            let password = $("#password").val().trim();

            // Client-side validation
            if (email === "") {
                $(".loader-overlay").css("display", "none");
                showSmallError("Please enter Email Id");
                $("#emailid").focus();
                return;
            }
            if (password === "") {
                $(".loader-overlay").css("display", "none");
                showSmallError("Please enter Password");
                $("#password").focus();
                return;
            }

            // Prepare request model
            let loginDTO = {
                EmailId: email,
                Password: password
            };

            $.ajax({
                url: '/Auth/Login',
                type: 'POST',
                data: JSON.stringify(loginDTO),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (res) {
                    $(".loader-overlay").css("display", "none");

                    if (res.success) {
                        // ✅ No popup here — directly go to OTP phase
                        $("#otpphase").css("display", "block");
                        $("#login1").css("display", "none");
                    } else {
                        // ❌ Only error popup
                        showSmallError(res.msg || "Invalid credentials");
                    }
                },
                error: function (err) {
                    console.error(err);
                    $(".loader-overlay").css("display", "none");
                    showSmallError("Something went wrong. Please try again later.");
                }
            });
        }
    });

    // 🔹 Helper function for small toast popups
    function showSmallError(message) {
        Swal.fire({
            toast: true,
            position: 'top-end',
            icon: 'error',
            title: message,
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true,
            background: '#ffffff',
            color: '#333',
            width: '250px'
        });
    }

 

});

function validateOtp() {
    let otp = $("#OTP").val().trim();
    $("#otpError").text("");

    if (otp === "") {
        $("#otpError").text("OTP is required");
        return false;
    } else if (!/^[0-9]{4}$/.test(otp)) {
        $("#otpError").text("Enter valid 4 digit OTP");
        return false;
    }
    return true;
}

$("#btnVerifyOtp").click(function () {
    if (validateOtp()) {
        $(".loader-overlay").css("display", "flex");

        // Simulate OTP verification
        setTimeout(() => {
            $(".loader-overlay").css("display", "none");

            let otp = $("#OTP").val().trim();

            if (otp === "1234") {
                // ✅ Correct OTP
                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'success',
                    title: 'Login successfully!',
                    showConfirmButton: false,
                    timer: 600,
                    timerProgressBar: true,
                    background: '#ffffff',
                    color: '#333',
                    width: '250px'
                });

                // Redirect after toast
                setTimeout(() => {
                    window.location.href = "/Home/Index";
                }, 600);
            } else {
                // ❌ Incorrect OTP
                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'error',
                    title: 'Incorrect OTP. Please try again.',
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true,
                    background: '#ffffff',
                    color: '#333',
                    width: '250px'
                });
            }
        }, 700);
    }
});

$("#btnResendOtp").click(function () {
    Swal.fire({
        toast: true,
        position: 'top-end',
        icon: 'info',
        title: 'OTP resent successfully!',
        showConfirmButton: false,
        timer: 2000,
        timerProgressBar: true,
        background: '#ffffff',
        color: '#333',
        width: '250px'
    });
    $(".loader-overlay").css("display", "none");
});
