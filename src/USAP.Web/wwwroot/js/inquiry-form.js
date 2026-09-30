document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('inquiryForm');
    if (!form) return;

    // --- 1. Dynamic Quote Fields Toggle (When Type Dropdown is Present) ---
    const typeSelect = form.querySelector('.js-inquiry-type-select');
    const quoteFields = form.querySelector('.js-quote-fields');

    if (typeSelect && quoteFields) {
        const quoteValue = form.getAttribute('data-quote-value');
        const orgLabel = form.querySelector('label[for="Organization"]');
        const orgInput = form.querySelector('#Organization') || form.querySelector('input[name="Organization"]');

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

        typeSelect.addEventListener('change', toggleQuoteFields);
        toggleQuoteFields();
    }

    // --- 2. Score-Based Google Cloud reCAPTCHA Enterprise Integration ---
    const isRecaptchaEnabled = form.getAttribute('data-recaptcha-enabled') === 'true';
    if (!isRecaptchaEnabled) return;

    const siteKey = form.getAttribute('data-recaptcha-site-key');
    const action = form.getAttribute('data-recaptcha-action') || 'contact_submit';
    const tokenInput = document.getElementById('recaptchaToken') || form.querySelector('input[name="RecaptchaToken"]');
    const clientError = document.getElementById('recaptchaClientError');
    const submitBtn = form.querySelector('button[type="submit"]');

    let isSubmitting = false;
    let hasAcquiredToken = false;

    const showClientError = () => {
        if (clientError) {
            clientError.style.display = 'block';
            clientError.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
        }
        isSubmitting = false;
        hasAcquiredToken = false;
        if (submitBtn) {
            submitBtn.disabled = false;
        }
    };

    form.addEventListener('submit', (e) => {
        if (hasAcquiredToken) {
            // Token has been successfully placed in the form; allow guarded submission to proceed
            return;
        }

        // Preserve native HTML5 form validation
        if (typeof form.checkValidity === 'function' && !form.checkValidity()) {
            return;
        }

        // Intercept form submission to acquire fresh token
        e.preventDefault();

        if (isSubmitting) {
            return;
        }

        isSubmitting = true;
        if (submitBtn) {
            submitBtn.disabled = true;
        }
        if (clientError) {
            clientError.style.display = 'none';
        }

        // Verify reCAPTCHA Enterprise script availability
        if (!window.grecaptcha || !window.grecaptcha.enterprise || typeof window.grecaptcha.enterprise.ready !== 'function') {
            showClientError();
            return;
        }

        try {
            window.grecaptcha.enterprise.ready(() => {
                try {
                    window.grecaptcha.enterprise.execute(siteKey, { action: action })
                        .then((token) => {
                            if (!token || typeof token !== 'string') {
                                showClientError();
                                return;
                            }

                            if (tokenInput) {
                                tokenInput.value = token;
                            }

                            hasAcquiredToken = true;
                            if (typeof form.requestSubmit === 'function') {
                                form.requestSubmit();
                            } else {
                                form.submit();
                            }
                        })
                        .catch(() => {
                            showClientError();
                        });
                } catch {
                    showClientError();
                }
            });
        } catch {
            showClientError();
        }
    });
});
