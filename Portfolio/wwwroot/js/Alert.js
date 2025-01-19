$(document).ready(function () {
    function fadeOutAlert() {
        const alertMessage = $("#alertMessage");

        if (alertMessage.length) {
                setTimeout(function () {
                    alertMessage.hide(); // Hide the message completely after fade-out
                }, 800); // Matches the duration of the fade-out
        }
    }

    // Call fadeOutAlert immediately
    fadeOutAlert();
});
