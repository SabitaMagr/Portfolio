$(document).ready(function () {
    // Attach togglePassword function to the eye icon
    $("#PasswordClick").on("click", function () {
        const passwordInput = $("#Password");
        if (passwordInput.attr("type") === "password") {
            passwordInput.attr("type", "text"); // Show password
        } else {
            passwordInput.attr("type", "password"); // Hide password
        }
    });
    $("#ConfirmPasswordClick").on("click", function () {
        const passwordInput = $("#ConfirmPassword");
        if (passwordInput.attr("type") === "password") {
            passwordInput.attr("type", "text"); // Show password
        } else {
            passwordInput.attr("type", "password"); // Hide password
        }
    });


});
