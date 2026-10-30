// Portfolio site scripts

// Highlight the active nav item while scrolling through the one-page sections.
const nav = document.getElementById('siteNav');
if (nav && window.bootstrap) {
    // Scrollspy needs position: relative on the scrolling root to track correctly.
    document.body.style.position = 'relative';
    new bootstrap.ScrollSpy(document.body, {
        target: '#siteNav',
        rootMargin: '0px 0px -70% 0px'
    });
}

// Keep the footer year current.
const year = document.getElementById('year');
if (year) {
    year.textContent = new Date().getFullYear();
}
