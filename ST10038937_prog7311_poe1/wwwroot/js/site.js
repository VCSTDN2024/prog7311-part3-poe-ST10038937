document.addEventListener('DOMContentLoaded', function () {
    const cultureSelect = document.getElementById('cultureSelect');

    if (cultureSelect) {
        cultureSelect.addEventListener('change', function () {
            const culture = this.value;
            const returnUrl = window.location.pathname + window.location.search;

            // 1. Set the language preference cookie in the background
            const setLangRequest = new XMLHttpRequest();
            setLangRequest.open('POST', '/Home/SetLanguage', true);
            setLangRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            setLangRequest.setRequestHeader('X-Requested-With', 'XMLHttpRequest'); // Identify as AJAX
            setLangRequest.send(`culture=${culture}&returnUrl=${encodeURIComponent(returnUrl)}`);

            // 2. Get all elements that need translation
            const elementsToTranslate = document.querySelectorAll('[data-translate-key]');
            const keys = Array.from(elementsToTranslate).map(el => el.getAttribute('data-translate-key'));

            // 3. Fetch the translations for the collected keys
            if (keys.length > 0) {
                const query = keys.map(k => `keys=${encodeURIComponent(k)}`).join('&');
                const getTranslationsRequest = new XMLHttpRequest();
                
                getTranslationsRequest.open('GET', `/Home/GetTranslations?${query}`, true);
                getTranslationsRequest.onreadystatechange = function() {
                    if (this.readyState === 4 && this.status === 200) {
                        const translations = JSON.parse(this.responseText);
                        
                        // 4. Update the UI with the new translations
                        elementsToTranslate.forEach(el => {
                            const key = el.getAttribute('data-translate-key');
                            if (translations[key]) {
                                el.textContent = translations[key];
                            }
                        });
                    }
                };
                getTranslationsRequest.send();
            }
        });
    }
});
