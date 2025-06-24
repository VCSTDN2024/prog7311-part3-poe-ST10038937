// Custom JavaScript for ST10038937 Agri-Energy Connect Platform
document.addEventListener('DOMContentLoaded', function () {

    // --- AJAX Language Switcher ---
    const cultureSelect = document.getElementById('cultureSelect');
    if (cultureSelect) {
        cultureSelect.addEventListener('change', function () {
            const culture = this.value;
            const returnUrl = window.location.pathname + window.location.search;

            // 1. Set the language culture cookie via an AJAX call
            fetch('/Home/SetLanguage', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'X-Requested-With': 'XMLHttpRequest' // Important for the server to know it's an AJAX call
                },
                body: `culture=${encodeURIComponent(culture)}&returnUrl=${encodeURIComponent(returnUrl)}`
            })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                // 2. After successfully setting the cookie, fetch the new translations
                fetchTranslations();
            })
            .catch(error => console.error('Error setting language:', error));
        });
    }

    function fetchTranslations() {
        // 3. Find all elements that need translation
        const elementsToTranslate = document.querySelectorAll('[data-translate-key]');
        const keys = Array.from(elementsToTranslate).map(el => el.getAttribute('data-translate-key'));

        if (keys.length === 0) return;

        // 4. Fetch the translations for these keys from the server
        const query = keys.map(key => `keys=${encodeURIComponent(key)}`).join('&');
        fetch(`/Home/GetTranslations?${query}`)
            .then(response => response.json())
            .then(translations => {
                // 5. Update the text of each element with the new translation
                elementsToTranslate.forEach(el => {
                    const key = el.getAttribute('data-translate-key');
                    if (translations[key]) {
                        el.textContent = translations[key];
                    }
                });
            })
            .catch(error => console.error('Error fetching translations:', error));
    }

    // --- Element Hiding & DOM Fixes ---
    const runDomFixes = function () {
        // Hide the external authentication section on the login page
        const sections = document.querySelectorAll('section');
        sections.forEach(function (section) {
            if (section.textContent.includes('Use another service to log in')) {
                section.style.display = 'none';
            }
        });

        // Hide the "Resend email confirmation" link
        const links = document.querySelectorAll('a');
        links.forEach(function (link) {
            if (link.id === 'resend-confirmation' || (link.href && link.href.includes('ResendEmailConfirmation'))) {
                link.style.display = 'none';
                if (link.parentElement && link.parentElement.tagName === 'P') {
                    link.parentElement.style.display = 'none';
                }
            }
        });

        // Fix currency displays
        const cells = document.querySelectorAll('td, span, dd');
        cells.forEach(function (cell) {
            if (cell.textContent.includes('R $') || cell.textContent.includes('R$')) {
                cell.textContent = cell.textContent.replace(/R \$/g, 'R ').replace(/R\$/g, 'R');
            }
            if (cell.textContent.startsWith('$')) {
                cell.textContent = 'R ' + cell.textContent.substring(1);
            }
        });
    };

    // Initial call
    runDomFixes();

    // Observer for dynamically loaded content
    const observer = new MutationObserver(function () {
        runDomFixes();
    });

    observer.observe(document.body, {
        childList: true,
        subtree: true
    });
});
