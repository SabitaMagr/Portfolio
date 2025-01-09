$(document).ready(function () {
    function fadeOutAlert() {
        const alertMessage = $("#alertMessage");
        if (alertMessage.length) {
            setTimeout(function () {
                alertMessage.addClass("fade-out");
                setTimeout(function () {
                    alertMessage.hide(); // Hide the message completely after fade-out
                }, 200); // Matches the duration of the fade-out
            }, 3000);
        }
    }

    // Call fadeOutAlert immediately
    fadeOutAlert();
});