/**
 * disclosure.js — Progressive enhancement animation for <details>/<summary> disclosures.
 * Handles .product-group-disclosure and .faq-item without third-party dependencies.
 * Fully reversible, responsive-safe, and prefers-reduced-motion compliant.
 */
(() => {
    'use strict';

    const prefersReducedMotion = () =>
        window.matchMedia('(prefers-reduced-motion: reduce)').matches;

    const initDisclosure = (details) => {
        const summary = details.querySelector('summary');
        if (!summary) return;

        const content = details.querySelector('.product-group-disclosure__content, .faq-content');
        if (!content) return;

        let activeAnimation = null;
        let isClosing = false;

        summary.addEventListener('click', (e) => {
            // If user prefers reduced motion, let native <details> toggle immediately
            if (prefersReducedMotion()) {
                return;
            }

            e.preventDefault();

            // Read current rendered geometry before making changes
            const computedStyle = window.getComputedStyle(content);
            const currentHeight = content.getBoundingClientRect().height;
            const currentOpacity = parseFloat(computedStyle.opacity) || 0;

            const wasOpen = details.open;
            if (!wasOpen) {
                details.open = true;
            }
            content.style.height = 'auto';
            content.style.overflow = 'visible';
            const fullHeight = content.scrollHeight;

            // Determine target state
            let targetOpen;
            if (activeAnimation && (activeAnimation.playState === 'running' || activeAnimation.playState === 'pending')) {
                targetOpen = isClosing; // reverse current transition
                activeAnimation.cancel();
                activeAnimation = null;
            } else {
                targetOpen = !wasOpen;
            }

            isClosing = !targetOpen;
            details.open = true;
            content.style.overflow = 'hidden';

            const startH = (activeAnimation || wasOpen) ? Math.min(currentHeight, fullHeight) : 0;
            const startOp = (activeAnimation || wasOpen) ? currentOpacity : 0;

            if (targetOpen) {
                const distance = Math.max(0, fullHeight - startH);
                const duration = Math.min(250, Math.max(90, Math.round(250 * (distance / (fullHeight || 1)))));

                activeAnimation = content.animate(
                    [
                        { height: `${startH}px`, opacity: startOp },
                        { height: `${fullHeight}px`, opacity: 1 }
                    ],
                    {
                        duration: duration,
                        easing: 'cubic-bezier(0.2, 0, 0, 1)'
                    }
                );

                activeAnimation.onfinish = () => {
                    activeAnimation = null;
                    isClosing = false;
                    content.style.height = '';
                    content.style.opacity = '';
                    content.style.overflow = '';
                };

                activeAnimation.oncancel = () => {
                    activeAnimation = null;
                };
            } else {
                const distance = Math.max(0, startH);
                const duration = Math.min(220, Math.max(80, Math.round(220 * (distance / (fullHeight || 1)))));

                activeAnimation = content.animate(
                    [
                        { height: `${startH}px`, opacity: startOp },
                        { height: '0px', opacity: 0 }
                    ],
                    {
                        duration: duration,
                        easing: 'cubic-bezier(0.2, 0, 0, 1)'
                    }
                );

                activeAnimation.onfinish = () => {
                    activeAnimation = null;
                    isClosing = false;
                    details.open = false;
                    content.style.height = '';
                    content.style.opacity = '';
                    content.style.overflow = '';
                };

                activeAnimation.oncancel = () => {
                    activeAnimation = null;
                };
            }
        });
    };

    const init = () => {
        const disclosures = document.querySelectorAll('.product-group-disclosure, .faq-item');
        disclosures.forEach(initDisclosure);
    };

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
