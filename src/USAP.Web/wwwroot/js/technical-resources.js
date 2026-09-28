/**
 * Technical Resources Library Client-Side Filtering & Search
 * Progressive enhancement for the /technical-resources page (Variant B Grouped Directory).
 */
(function () {
  'use strict';

  function initTechnicalResources() {
    var searchInput = document.getElementById('tech-search-input');
    var searchForm = document.getElementById('tech-search-form');
    var clearSearchBtn = document.getElementById('clear-search-btn');
    var filterPills = document.querySelectorAll('.filter-pill');
    var panels = document.querySelectorAll('.resource-document-row, .resource-panel, .resource-card');
    var familyGroups = document.querySelectorAll('.resource-family, .resource-family-group');
    var emptyState = document.getElementById('resources-empty');
    var visibleCountEl = document.getElementById('visible-count');
    var liveStatusEl = document.getElementById('search-live-status');
    var resetBtn = document.getElementById('reset-filters-btn');
    var toolbarResetBtn = document.getElementById('toolbar-reset-btn');
    var activeFilterBanner = document.getElementById('active-filter-banner');
    var activeFilterLabel = document.getElementById('active-filter-label');
    var activeFilterClearBtn = document.getElementById('active-filter-clear-btn');

    if (!panels.length) {
      return;
    }

    var totalCount = panels.length;
    var currentFamily = 'all';
    var currentType = '';
    var currentView = '';

    // Parse URL search parameters on load
    var urlParams = new URLSearchParams(window.location.search);
    var paramCategory = urlParams.get('category') || urlParams.get('family');
    var paramType = urlParams.get('type');
    var paramView = urlParams.get('view');
    var paramQ = urlParams.get('q');

    if (paramView && paramView.toLowerCase() === 'antenna-systems') {
      currentView = 'antenna-systems';
    } else if (paramType && paramType.toLowerCase() === 'data-sheet') {
      currentType = 'data-sheet';
    } else if (paramCategory) {
      currentFamily = paramCategory.toLowerCase();
    }

    if (paramQ && searchInput && !searchInput.value) {
      searchInput.value = paramQ;
    }

    // Initialize family pill state
    filterPills.forEach(function (pill) {
      var pillFamily = (pill.getAttribute('data-family') || '').toLowerCase();
      if (!currentType && !currentView && pillFamily === currentFamily) {
        pill.classList.add('filter-pill--active');
        pill.setAttribute('aria-pressed', 'true');
      } else {
        pill.classList.remove('filter-pill--active');
        pill.setAttribute('aria-pressed', 'false');
      }
    });

    var currentQuery = searchInput ? searchInput.value.trim().toLowerCase() : '';

    function updateActiveFilterBanner() {
      if (!activeFilterBanner) return;
      if (currentView === 'antenna-systems') {
        if (activeFilterLabel) activeFilterLabel.textContent = 'Antenna Systems';
        activeFilterBanner.classList.remove('active-filter-banner--hidden');
      } else if (currentType === 'data-sheet') {
        if (activeFilterLabel) activeFilterLabel.textContent = 'Product Data Sheets';
        activeFilterBanner.classList.remove('active-filter-banner--hidden');
      } else {
        activeFilterBanner.classList.add('active-filter-banner--hidden');
      }
    }

    function updateClearButton() {
      if (!clearSearchBtn || !searchInput) return;
      if (searchInput.value.length > 0) {
        clearSearchBtn.removeAttribute('hidden');
      } else {
        clearSearchBtn.setAttribute('hidden', '');
      }
    }

    function applyFilters() {
      var queryTerms = currentQuery.length > 0
        ? currentQuery.split(/\s+/).filter(function (t) { return t.length > 0; })
        : [];

      var visibleCount = 0;

      panels.forEach(function (panel) {
        var cardFamily = (panel.getAttribute('data-family') || '').toLowerCase();
        var cardType = (panel.getAttribute('data-type') || '').toLowerCase();
        var cardIsAntenna = panel.getAttribute('data-is-antenna') === 'true';
        var cardSearch = (panel.getAttribute('data-search') || '') + ' ' +
                         (panel.getAttribute('data-title') || '') + ' ' +
                         (panel.getAttribute('data-models') || '');

        var matchesCategory = true;
        if (currentView === 'antenna-systems') {
          matchesCategory = cardIsAntenna;
        } else if (currentType === 'data-sheet') {
          matchesCategory = (cardType === 'data-sheet');
        } else if (currentFamily !== 'all') {
          matchesCategory = (cardFamily === currentFamily);
        }

        var matchesQuery = true;
        if (queryTerms.length > 0) {
          for (var i = 0; i < queryTerms.length; i++) {
            if (cardSearch.indexOf(queryTerms[i]) === -1) {
              matchesQuery = false;
              break;
            }
          }
        }

        if (matchesCategory && matchesQuery) {
          panel.classList.remove('resource-document-row--hidden');
          panel.classList.remove('resource-panel--hidden');
          panel.classList.remove('resource-card--hidden');
          visibleCount++;
        } else {
          panel.classList.add('resource-document-row--hidden');
          panel.classList.add('resource-panel--hidden');
          panel.classList.add('resource-card--hidden');
        }
      });

      // Update family groups visibility and section count labels
      familyGroups.forEach(function (group) {
        var groupPanels = group.querySelectorAll('.resource-document-row, .resource-panel, .resource-card');
        var groupVisibleCount = 0;
        groupPanels.forEach(function (p) {
          if (!p.classList.contains('resource-document-row--hidden') &&
              !p.classList.contains('resource-panel--hidden') &&
              !p.classList.contains('resource-card--hidden')) {
            groupVisibleCount++;
          }
        });

        if (groupVisibleCount > 0) {
          group.classList.remove('resource-family--hidden');
          group.classList.remove('resource-family-group--hidden');
          var countEl = group.querySelector('.family-visible-count');
          if (countEl) {
            countEl.textContent = groupVisibleCount;
          }
          var labelEl = group.querySelector('.family-count-label');
          if (labelEl) {
            labelEl.textContent = groupVisibleCount === 1 ? 'DOCUMENT' : 'DOCUMENTS';
          }
        } else {
          group.classList.add('resource-family--hidden');
          group.classList.add('resource-family-group--hidden');
        }
      });

      // Update counters
      if (visibleCountEl) {
        visibleCountEl.textContent = visibleCount;
      }

      if (liveStatusEl) {
        liveStatusEl.textContent = 'Showing ' + visibleCount + ' of ' + totalCount + ' technical documents';
      }

      // Empty state
      if (visibleCount === 0) {
        if (emptyState) {
          emptyState.classList.remove('resources-empty--hidden');
        }
      } else {
        if (emptyState) {
          emptyState.classList.add('resources-empty--hidden');
        }
      }

      // Reset button visibility (hide if preset banner already provides Clear filter)
      var hasActivePreset = (currentType !== '' || currentView !== '');
      var isFiltered = (currentFamily !== 'all' || currentQuery.length > 0) && !hasActivePreset;
      if (toolbarResetBtn) {
        if (isFiltered) {
          toolbarResetBtn.removeAttribute('hidden');
        } else {
          toolbarResetBtn.setAttribute('hidden', '');
        }
      }

      updateActiveFilterBanner();
      updateClearButton();
    }

    // Intercept form submission to keep client-side filtering smooth
    if (searchForm) {
      searchForm.addEventListener('submit', function (e) {
        e.preventDefault();
        if (searchInput) {
          currentQuery = searchInput.value.trim().toLowerCase();
        }
        applyFilters();
      });
    }

    // Live search input
    if (searchInput) {
      searchInput.addEventListener('input', function () {
        currentQuery = searchInput.value.trim().toLowerCase();
        applyFilters();
      });

      searchInput.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
          if (searchInput.value.length > 0) {
            searchInput.value = '';
            currentQuery = '';
            applyFilters();
          } else {
            resetAllFilters();
          }
        }
      });
    }

    // Clear search button
    if (clearSearchBtn) {
      clearSearchBtn.addEventListener('click', function () {
        if (searchInput) {
          searchInput.value = '';
          currentQuery = '';
          searchInput.focus();
        }
        applyFilters();
      });
    }

    // Category filter pills
    filterPills.forEach(function (pill, index) {
      pill.addEventListener('click', function () {
        currentType = '';
        currentView = '';

        filterPills.forEach(function (p) {
          p.classList.remove('filter-pill--active');
          p.setAttribute('aria-pressed', 'false');
        });

        pill.classList.add('filter-pill--active');
        pill.setAttribute('aria-pressed', 'true');
        currentFamily = (pill.getAttribute('data-family') || 'all').toLowerCase();
        applyFilters();
      });

      // Arrow navigation for keyboard accessibility
      pill.addEventListener('keydown', function (e) {
        var targetIndex = -1;
        if (e.key === 'ArrowRight' || e.key === 'ArrowDown') {
          e.preventDefault();
          targetIndex = (index + 1) % filterPills.length;
        } else if (e.key === 'ArrowLeft' || e.key === 'ArrowUp') {
          e.preventDefault();
          targetIndex = (index - 1 + filterPills.length) % filterPills.length;
        }

        if (targetIndex >= 0) {
          filterPills[targetIndex].focus();
        }
      });
    });

    // Reset filters helper
    function resetAllFilters(e) {
      if (e && e.preventDefault) e.preventDefault();
      currentFamily = 'all';
      currentType = '';
      currentView = '';
      currentQuery = '';
      if (searchInput) {
        searchInput.value = '';
      }

      filterPills.forEach(function (p) {
        var isAll = (p.getAttribute('data-family') === 'all');
        if (isAll) {
          p.classList.add('filter-pill--active');
          p.setAttribute('aria-pressed', 'true');
        } else {
          p.classList.remove('filter-pill--active');
          p.setAttribute('aria-pressed', 'false');
        }
      });

      applyFilters();
      if (searchInput) {
        searchInput.focus();
      }
    }

    if (resetBtn) {
      resetBtn.addEventListener('click', resetAllFilters);
    }

    if (toolbarResetBtn) {
      toolbarResetBtn.addEventListener('click', resetAllFilters);
    }

    if (activeFilterClearBtn) {
      activeFilterClearBtn.addEventListener('click', resetAllFilters);
    }

    // Initial pass
    applyFilters();
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initTechnicalResources);
  } else {
    initTechnicalResources();
  }
})();
