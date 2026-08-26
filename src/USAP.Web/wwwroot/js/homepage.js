// homepage.js - Homepage specific behaviors (Hero progressive enhancement)

document.addEventListener("DOMContentLoaded", function () {
    const video = document.getElementById("hero-video");
    const motionPref = window.matchMedia("(prefers-reduced-motion: reduce)");

    // No video if missing
    if (!video) return;

    // We need to re-add the playing listener if we re-attempt playback
    const onPlaying = function () {
        video.classList.add("is-ready");
    };
    video.addEventListener("playing", onPlaying);

    const attemptPlay = function() {
        video.removeAttribute("hidden");
        const playPromise = video.play();
        if (playPromise !== undefined) {
            playPromise.catch(function () {
                // Autoplay rejected or failed silently (per requirements avoid unnecessary warnings unless required)
                video.classList.remove("is-ready");
                video.setAttribute("hidden", "true");
            });
        }
    };

    // Initial load handling
    if (!motionPref.matches) {
        attemptPlay();
    }

    // Handle later media errors
    video.addEventListener("error", function () {
        video.classList.remove("is-ready");
        video.setAttribute("hidden", "true");
    });

    // Respond safely if motion preference changes while on page
    motionPref.addEventListener("change", function (e) {
        if (e.matches) {
            // User requested reduced motion -> stop video
            video.pause();
            video.classList.remove("is-ready");
            video.setAttribute("hidden", "true");
        } else {
            // User allowed motion -> attempt play
            attemptPlay();
        }
    });
});
