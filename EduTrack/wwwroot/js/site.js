document.addEventListener("DOMContentLoaded", function () {

    const toggle = document.getElementById("themeToggle");

    // Apply saved theme on load
    const savedTheme = localStorage.getItem("theme");

    if (savedTheme === "dark") {
        applyDarkMode();
        if (toggle) toggle.checked = true;
    } else {
        applyLightMode();
        if (toggle) toggle.checked = false;
    }

    // Attach toggle event
    if (toggle) {
        toggle.addEventListener("change", function () {
            if (this.checked) {
                applyDarkMode();
                localStorage.setItem("theme", "dark");
            } else {
                applyLightMode();
                localStorage.setItem("theme", "light");
            }
        });
    }
});

function applyDarkMode() {
    document.documentElement.setAttribute('data-bs-theme', 'dark');
    const navbar = document.querySelector(".navbar");
    if (navbar) {
        navbar.classList.add("navbar-dark", "bg-dark", "border-secondary");
        navbar.classList.remove("navbar-light", "bg-white");
    }
}

function applyLightMode() {
    document.documentElement.setAttribute('data-bs-theme', 'light');
    const navbar = document.querySelector(".navbar");
    if (navbar) {
        navbar.classList.remove("navbar-dark", "bg-dark", "border-secondary");
        navbar.classList.add("navbar-light", "bg-white", "border-bottom");
    }
}
