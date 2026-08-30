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
    const testimonialSection = document.querySelector(".home-testimonials");
    if (testimonialSection) {
        const carousel = testimonialSection.querySelector("#testimonial-carousel");
        const slides = Array.from(carousel.querySelectorAll(".testimonial-slide"));
        const indicators = Array.from(testimonialSection.querySelectorAll(".carousel-indicators .indicator"));
        const prevBtn = testimonialSection.querySelector(".carousel-prev");
        const nextBtn = testimonialSection.querySelector(".carousel-next");
        let currentIndex = 0;
        let autoplayInterval = null;
        let isHovered = false;
        let isFocused = false;

        function startAutoplay() {
            if (slides.length < 2) return;
            if (isHovered || isFocused || document.hidden) return;
            if (autoplayInterval) return;

            autoplayInterval = setInterval(() => {
                nextSlide();
            }, 8000);
        }

        function stopAutoplay() {
            if (autoplayInterval) {
                clearInterval(autoplayInterval);
                autoplayInterval = null;
            }
        }

        function resetAutoplay() {
            stopAutoplay();
            startAutoplay();
        }

        let isTransitioning = false;

        function updateIndicators(index) {
            indicators.forEach((indicator, i) => {
                if (i === index) {
                    indicator.classList.add("active");
                    indicator.setAttribute("aria-selected", "true");
                } else {
                    indicator.classList.remove("active");
                    indicator.setAttribute("aria-selected", "false");
                }
            });
        }

        async function showSlide(index, direction = "forward") {
            if (index === currentIndex || isTransitioning) return;

            const outgoing = slides[currentIndex];
            const incoming = slides[index];

            if (!outgoing.animate || typeof Promise === "undefined") {
                // Immediate fallback
                outgoing.classList.remove("active");
                outgoing.setAttribute("aria-hidden", "true");
                incoming.classList.add("active");
                incoming.setAttribute("aria-hidden", "false");
                updateIndicators(index);
                currentIndex = index;
                return;
            }

            isTransitioning = true;
            const outX = direction === "forward" ? "-8px" : "8px";
            const inX = direction === "forward" ? "8px" : "-8px";

            let outAnim, inAnim;
            try {
                // Phase 1: Outgoing slide
                outAnim = outgoing.animate([
                    { opacity: 1, transform: "translateX(0)" },
                    { opacity: 0, transform: `translateX(${outX})` }
                ], {
                    duration: 150,
                    easing: "ease-in",
                    fill: "forwards"
                });

                await outAnim.finished;

                outgoing.classList.remove("active");
                outgoing.setAttribute("aria-hidden", "true");
                incoming.classList.add("active");
                incoming.setAttribute("aria-hidden", "false");

                // Phase 2: Incoming slide
                inAnim = incoming.animate([
                    { opacity: 0, transform: `translateX(${inX})` },
                    { opacity: 1, transform: "translateX(0)" }
                ], {
                    duration: 230,
                    easing: "cubic-bezier(0.22, 1, 0.36, 1)",
                    fill: "forwards"
                });

                await inAnim.finished;
            } catch (e) {
                // Ignore animation cancellations or failures safely
            } finally {
                outgoing.classList.remove("active");
                outgoing.setAttribute("aria-hidden", "true");
                incoming.classList.add("active");
                incoming.setAttribute("aria-hidden", "false");
                if (outAnim) outAnim.cancel();
                if (inAnim) inAnim.cancel();
                updateIndicators(index);
                currentIndex = index;
                isTransitioning = false;
            }
        }

        function nextSlide() {
            let nextIndex = currentIndex + 1;
            if (nextIndex >= slides.length) {
                nextIndex = 0;
            }
            showSlide(nextIndex, "forward");
        }

        function prevSlide() {
            let prevIndex = currentIndex - 1;
            if (prevIndex < 0) {
                prevIndex = slides.length - 1;
            }
            showSlide(prevIndex, "backward");
        }

        if (nextBtn) {
            nextBtn.addEventListener("click", () => {
                nextSlide();
                resetAutoplay();
            });
        }

        if (prevBtn) {
            prevBtn.addEventListener("click", () => {
                prevSlide();
                resetAutoplay();
            });
        }

        indicators.forEach((indicator, i) => {
            indicator.addEventListener("click", () => {
                if (i === currentIndex) return;
                const direction = i > currentIndex ? "forward" : "backward";
                showSlide(i, direction);
                resetAutoplay();
            });
        });

        carousel.addEventListener("keydown", function (e) {
            if (e.key === "ArrowLeft") {
                prevSlide();
                resetAutoplay();
            } else if (e.key === "ArrowRight") {
                nextSlide();
                resetAutoplay();
            }
        });

        testimonialSection.addEventListener("mouseenter", () => {
            isHovered = true;
            stopAutoplay();
        });

        testimonialSection.addEventListener("mouseleave", () => {
            isHovered = false;
            startAutoplay();
        });

        testimonialSection.addEventListener("focusin", () => {
            isFocused = true;
            stopAutoplay();
        });

        testimonialSection.addEventListener("focusout", (e) => {
            if (!testimonialSection.contains(e.relatedTarget)) {
                isFocused = false;
                startAutoplay();
            }
        });

        document.addEventListener("visibilitychange", () => {
            if (document.hidden) {
                stopAutoplay();
            } else {
                startAutoplay();
            }
        });

        startAutoplay();

        // Make carousel focusable for keyboard events if not already
        carousel.setAttribute("tabindex", "0");
    }
