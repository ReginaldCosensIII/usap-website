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

    // Testimonials Carousel
    const carousel = document.getElementById("testimonial-carousel");
    if (carousel) {
        const slides = Array.from(carousel.querySelectorAll(".testimonial-slide"));
        const indicators = Array.from(document.querySelectorAll(".carousel-indicators .indicator"));
        const prevBtn = document.querySelector(".carousel-prev");
        const nextBtn = document.querySelector(".carousel-next");
        let currentIndex = 0;

        function showSlide(index) {
            slides.forEach((slide, i) => {
                if (i === index) {
                    slide.classList.add("active");
                    slide.removeAttribute("hidden");
                } else {
                    slide.classList.remove("active");
                    slide.setAttribute("hidden", "true");
                }
            });

            indicators.forEach((indicator, i) => {
                if (i === index) {
                    indicator.classList.add("active");
                    indicator.setAttribute("aria-selected", "true");
                } else {
                    indicator.classList.remove("active");
                    indicator.setAttribute("aria-selected", "false");
                }
            });
            currentIndex = index;
        }

        function nextSlide() {
            let nextIndex = currentIndex + 1;
            if (nextIndex >= slides.length) {
                nextIndex = 0;
            }
            showSlide(nextIndex);
        }

        function prevSlide() {
            let prevIndex = currentIndex - 1;
            if (prevIndex < 0) {
                prevIndex = slides.length - 1;
            }
            showSlide(prevIndex);
        }

        if (nextBtn) {
            nextBtn.addEventListener("click", nextSlide);
        }

        if (prevBtn) {
            prevBtn.addEventListener("click", prevSlide);
        }

        indicators.forEach((indicator, i) => {
            indicator.addEventListener("click", () => {
                showSlide(i);
            });
        });

        carousel.addEventListener("keydown", function (e) {
            if (e.key === "ArrowLeft") {
                prevSlide();
            } else if (e.key === "ArrowRight") {
                nextSlide();
            }
        });

        // Make carousel focusable for keyboard events if not already
        carousel.setAttribute("tabindex", "0");
    }
