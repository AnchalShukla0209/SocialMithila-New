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

    $("#btnLogin").click(function () {
        if (validateLogin()) {
            $(".loader-overlay").css("display", "flex");
            let email = $("#emailid").val().trim();
            let password = $("#password").val().trim();

            // Client-side validation
            if (email === "") {
                alert("Please enter Email Id");
                $("#emailid").focus();
                return;
            }
            if (password === "") {
                alert("Please enter Password");
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
                    if (res.success) {
                        alert(res.msg);
                        $("#otpphase").css("display", "block");
                        $("#login1").css("display", "none");
                        $(".loader-overlay").css("display", "none");
                    } else {
                        alert(res.msg);
                        $(".loader-overlay").css("display", "none");
                    }
                },
                error: function (err) {
                    console.error(err);
                    alert("Something went wrong. Please try again later.");
                    $(".loader-overlay").css("display", "none");
                }
            });
        }
    });

   

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
        window.location.href = "/Home/Index";
    }
});

$("#btnResendOtp").click(function () {
    alert("OTP resent successfully!");
    $(".loader-overlay").css("display", "none");
});