document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('inquiryForm');
    if (!form) return;

    const typeSelect = form.querySelector('.js-inquiry-type-select');
    const quoteFields = form.querySelector('.js-quote-fields');

    if (!typeSelect || !quoteFields) return;

    const quoteValue = form.getAttribute('data-quote-value');
    const orgLabel = form.querySelector('label[for="Organization"]');
    const orgInput = form.querySelector('#Organization') || form.querySelector('input[name="Organization"]');

    // Helper to toggle visibility and required state
    const toggleQuoteFields = () => {
        const isQuote = typeSelect.value === quoteValue;
        quoteFields.style.display = isQuote ? 'block' : 'none';

        if (orgLabel) {
            let reqSpan = orgLabel.querySelector('.form-required');
            if (isQuote) {
                if (!reqSpan) {
                    reqSpan = document.createElement('span');
                    reqSpan.className = 'form-required';
                    reqSpan.setAttribute('aria-label', 'required');
                    reqSpan.textContent = ' *';
                    orgLabel.appendChild(reqSpan);
                }
                if (orgInput) {
                    orgInput.setAttribute('aria-required', 'true');
                }
            } else {
                if (reqSpan) {
                    reqSpan.remove();
                }
                if (orgInput) {
                    orgInput.removeAttribute('aria-required');
                }
            }
        }
    };

    // Listen for changes
    typeSelect.addEventListener('change', toggleQuoteFields);

    // Initial toggle
    toggleQuoteFields();
});
