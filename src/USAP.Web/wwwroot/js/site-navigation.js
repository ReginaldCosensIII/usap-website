/**
 * site-navigation.js
 * Dependency-free progressive-enhancement mobile navigation and persistent sticky header module.
 *
 * Behavior contract:
 * - Adds class 'js-nav-ready' to <html> so CSS can reveal the toggle and
 *   manage the nav overlay on mobile.
 * - Toggle has aria-expanded and aria-controls wired to the nav list.
 * - Mobile nav opens as a content-height overlay positioned below the mobile header
 *   using stable header offsetHeight (--mobile-header-bottom).
 * - Click-away backdrop dims the page beneath and dismisses the nav on tap/click.
 * - Locks background scroll without scroll drift; preserves and restores scroll position.
 * - Applies inert to background regions (#main-content, .site-footer, .skip-link)
 *   while open to contain keyboard focus without interfering with dialog semantics.
 * - Escape closes the menu and returns focus to the toggle.
 * - Clicking any nav link closes the menu without restoring scroll to allow navigation.
 * - Resizing to desktop breakpoint (≥1024px) clears stale open state, scroll lock, and inert state.
 * - Native <details>/<summary> for Products dropdown operates without redundant ARIA overrides.
 * - Always-visible sticky header:
 *   - The shared site header remains position: sticky; top: 0 at all times.
 *   - Zero hide-on-scroll / auto-hide state machine or scroll listeners.
 */
(function () {
  'use strict';

  var DESKTOP_BREAKPOINT = 1024; // px — must match CSS lg breakpoint (64rem)

  var header = document.querySelector('.site-header');
  var toggle = document.getElementById('nav-toggle');
  var navList = document.getElementById('primary-nav-list');
  var backdrop = document.getElementById('nav-backdrop');
  var productsDropdown = document.getElementById('nav-products-dropdown');

  if (!toggle || !navList || !header) return;

  // Progressive enhancement: signal to CSS that JS is active
  document.documentElement.classList.add('js-nav-ready');

  var savedScrollY = null;
  var inertAppliedElements = [];
  var resizeTimer = null;

  function isDesktop() {
    return window.innerWidth >= DESKTOP_BREAKPOINT;
  }

  function isOpen() {
    return toggle.getAttribute('aria-expanded') === 'true';
  }

  function updateHeaderBottom() {
    if (header) {
      // Use stable layout height (offsetHeight) of always-visible sticky header
      var headerHeight = header.offsetHeight;
      document.documentElement.style.setProperty('--mobile-header-bottom', headerHeight + 'px');
    }
  }

  function applyBackgroundInert() {
    inertAppliedElements = [];
    var targets = [
      document.getElementById('main-content'),
      document.querySelector('.site-footer'),
      document.querySelector('.skip-link')
    ];
    targets.forEach(function (el) {
      if (el && !el.hasAttribute('inert')) {
        el.setAttribute('inert', '');
        inertAppliedElements.push(el);
      }
    });
  }

  function removeBackgroundInert() {
    inertAppliedElements.forEach(function (el) {
      if (el) {
        el.removeAttribute('inert');
      }
    });
    inertAppliedElements = [];
  }

  function openNav() {
    savedScrollY = window.scrollY || window.pageYOffset || document.documentElement.scrollTop || 0;

    // Compute overlay offset from stable header height
    updateHeaderBottom();

    // Lock background scroll
    document.documentElement.classList.add('nav-open-lock');
    document.body.classList.add('nav-open-lock');

    applyBackgroundInert();

    navList.removeAttribute('hidden');
    if (backdrop) {
      backdrop.classList.add('is-active');
    }

    toggle.setAttribute('aria-expanded', 'true');
    toggle.setAttribute('aria-label', 'Close navigation menu');
  }

  function closeNav(options) {
    var shouldRestoreScroll = !options || options.restoreScroll !== false;

    removeBackgroundInert();

    // Unlock background scroll
    document.documentElement.classList.remove('nav-open-lock');
    document.body.classList.remove('nav-open-lock');
    document.documentElement.style.removeProperty('--mobile-header-bottom');

    navList.setAttribute('hidden', '');
    if (backdrop) {
      backdrop.classList.remove('is-active');
    }

    toggle.setAttribute('aria-expanded', 'false');
    toggle.setAttribute('aria-label', 'Open navigation menu');

    // Restore scroll position without drift ONLY when requested and a saved position exists
    if (shouldRestoreScroll && savedScrollY !== null) {
      var restoreY = savedScrollY;
      savedScrollY = null;
      try {
        window.scrollTo({ top: restoreY, behavior: 'instant' });
      } catch (e) {
        window.scrollTo(0, restoreY);
      }
    } else {
      savedScrollY = null;
    }
  }

  // Initial state: close enhanced menu without restoring scroll, preserving fragments and browser scroll restoration
  navList.setAttribute('hidden', '');
  toggle.setAttribute('aria-expanded', 'false');
  toggle.setAttribute('aria-label', 'Open navigation menu');

  // Toggle button click
  toggle.addEventListener('click', function () {
    if (isOpen()) {
      closeNav();
    } else {
      openNav();
    }
  });

  // Click on backdrop dismisses nav
  if (backdrop) {
    backdrop.addEventListener('click', function () {
      if (isOpen()) {
        closeNav();
        toggle.focus();
      }
    });
  }

  // Escape key — close and return focus to toggle
  document.addEventListener('keydown', function (event) {
    if (event.key === 'Escape' && isOpen() && !isDesktop()) {
      closeNav();
      toggle.focus();
    }
  });

  // Click outside header region — close nav
  document.addEventListener('click', function (event) {
    if (isOpen() && header && !header.contains(event.target) && (!backdrop || event.target !== backdrop)) {
      closeNav();
    }
  });

  // Nav link selection — close the menu without restoring scroll to allow navigation
  var navLinks = navList.querySelectorAll('a');
  navLinks.forEach(function (link) {
    link.addEventListener('click', function () {
      if (!isDesktop()) {
        closeNav({ restoreScroll: false });
      }
    });
  });

  // Products dropdown enhancement (Desktop dismissals and keyboard return)
  if (productsDropdown) {
    var dropdownSummary = productsDropdown.querySelector('summary');

    // Dismiss dropdown on outside click (desktop)
    document.addEventListener('click', function (event) {
      if (productsDropdown.open && isDesktop() && !productsDropdown.contains(event.target)) {
        productsDropdown.removeAttribute('open');
      }
    });

    // Escape key closes dropdown and returns focus to summary
    document.addEventListener('keydown', function (event) {
      if (event.key === 'Escape' && productsDropdown.open) {
        productsDropdown.removeAttribute('open');
        if (dropdownSummary) {
          dropdownSummary.focus();
        }
        event.stopPropagation();
      }
    });

    // Clicking dropdown link closes dropdown on desktop
    var dropdownLinks = productsDropdown.querySelectorAll('a');
    dropdownLinks.forEach(function (link) {
      link.addEventListener('click', function () {
        if (isDesktop()) {
          productsDropdown.removeAttribute('open');
        }
      });
    });
  }

  // Resize / Orientation handling
  function handleViewportChange() {
    clearTimeout(resizeTimer);
    resizeTimer = setTimeout(function () {
      if (isDesktop()) {
        if (isOpen()) {
          closeNav({ restoreScroll: false });
        }
        removeBackgroundInert();
        document.documentElement.classList.remove('nav-open-lock');
        document.body.classList.remove('nav-open-lock');
        document.documentElement.style.removeProperty('--mobile-header-bottom');
        navList.removeAttribute('hidden');
        if (backdrop) {
          backdrop.classList.remove('is-active');
        }
        toggle.setAttribute('aria-expanded', 'false');
      } else if (isOpen()) {
        updateHeaderBottom();
      } else {
        navList.setAttribute('hidden', '');
        toggle.setAttribute('aria-expanded', 'false');
        toggle.setAttribute('aria-label', 'Open navigation menu');
        removeBackgroundInert();
        document.documentElement.classList.remove('nav-open-lock');
        document.body.classList.remove('nav-open-lock');
        document.documentElement.style.removeProperty('--mobile-header-bottom');
        if (backdrop) {
          backdrop.classList.remove('is-active');
        }
      }
    }, 50);
  }

  window.addEventListener('resize', handleViewportChange);
  window.addEventListener('orientationchange', handleViewportChange);
}());
