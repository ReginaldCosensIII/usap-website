document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('inquiryForm');
    if (!form) return;

    const typeSelect = form.querySelector('.js-inquiry-type-select');
    const quoteFields = form.querySelector('.js-quote-fields');

    if (!typeSelect || !quoteFields) return;

    const quoteValue = form.getAttribute('data-quote-value');

    // Helper to toggle visibility
    const toggleQuoteFields = () => {
        if (typeSelect.value === quoteValue) {
            quoteFields.style.display = 'block';
        } else {
            quoteFields.style.display = 'none';
        }
    };

    // Listen for changes
    typeSelect.addEventListener('change', toggleQuoteFields);

    // Initial toggle
    toggleQuoteFields();
});
