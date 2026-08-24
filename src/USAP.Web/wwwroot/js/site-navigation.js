/**
 * site-navigation.js
 * Dependency-free progressive-enhancement mobile navigation module.
 *
 * Behavior contract:
 * - Adds class 'js-nav-ready' to <html> so CSS can reveal the toggle and
 *   collapse the nav panel on mobile.
 * - Toggle has aria-expanded and aria-controls wired to the nav list.
 * - Escape closes the menu and returns focus to the toggle.
 * - Click outside the nav region closes the menu.
 * - Clicking any nav link closes the menu.
 * - Resizing to desktop breakpoint (≥1024px) clears stale open state.
 * - No focus trap — this is a disclosure, not a modal.
 * - No body-scroll locking.
 * - Respects prefers-reduced-motion (no animated height transitions added).
 */
(function () {
  'use strict';

  var DESKTOP_BREAKPOINT = 1024; // px — must match CSS lg breakpoint (64rem)

  var toggle = document.getElementById('nav-toggle');
  var navList = document.getElementById('primary-nav-list');

  if (!toggle || !navList) return;

  // Progressive enhancement: signal to CSS that JS is active
  document.documentElement.classList.add('js-nav-ready');

  // Collapse the nav panel initially on mobile
  function isDesktop() {
    return window.innerWidth >= DESKTOP_BREAKPOINT;
  }

  function openNav() {
    navList.removeAttribute('hidden');
    toggle.setAttribute('aria-expanded', 'true');
    toggle.setAttribute('aria-label', 'Close navigation menu');
  }

  function closeNav() {
    navList.setAttribute('hidden', '');
    toggle.setAttribute('aria-expanded', 'false');
    toggle.setAttribute('aria-label', 'Open navigation menu');
  }

  function isOpen() {
    return toggle.getAttribute('aria-expanded') === 'true';
  }

  // Initial state: closed on mobile, unconstrained on desktop
  if (!isDesktop()) {
    closeNav();
  }

  // Toggle on button click
  toggle.addEventListener('click', function () {
    if (isOpen()) {
      closeNav();
    } else {
      openNav();
    }
  });

  // Escape key — close and return focus to toggle
  document.addEventListener('keydown', function (event) {
    if (event.key === 'Escape' && isOpen() && !isDesktop()) {
      closeNav();
      toggle.focus();
    }
  });

  // Click outside — close the nav
  document.addEventListener('click', function (event) {
    var header = document.querySelector('.site-header');
    if (isOpen() && header && !header.contains(event.target)) {
      closeNav();
    }
  });

  // Nav link selection — close the menu
  var navLinks = navList.querySelectorAll('a');
  navLinks.forEach(function (link) {
    link.addEventListener('click', function () {
      if (!isDesktop()) {
        closeNav();
      }
    });
  });

  // Resize — clear stale mobile state when reaching desktop breakpoint
  var resizeTimer;
  window.addEventListener('resize', function () {
    clearTimeout(resizeTimer);
    resizeTimer = setTimeout(function () {
      if (isDesktop()) {
        // Remove hidden so the nav list is always visible at desktop
        navList.removeAttribute('hidden');
        toggle.setAttribute('aria-expanded', 'false');
      } else if (!isOpen()) {
        // Re-collapse if resized back to mobile while closed
        closeNav();
      }
    }, 100);
  });
}());
