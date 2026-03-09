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
    const body = document.body;
    const navbar = document.querySelector(".navbar");

    body.classList.add("bg-dark", "text-white");
    body.classList.remove("bg-white");

    if (navbar) {
        navbar.classList.add("navbar-dark", "bg-dark");
        navbar.classList.remove("navbar-light", "bg-white");
    }
}

function applyLightMode() {
    const body = document.body;
    const navbar = document.querySelector(".navbar");

    body.classList.remove("bg-dark", "text-white");
    body.classList.add("bg-white");

    if (navbar) {
        navbar.classList.remove("navbar-dark", "bg-dark");
        navbar.classList.add("navbar-light", "bg-white");
    }
}