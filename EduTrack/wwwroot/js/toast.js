// Simple Toast helper using Bootstrap Toasts
(function (window) {
    function ensureContainer() {
        let container = document.getElementById('toast-container');
        if (!container) {
            container = document.createElement('div');
            container.id = 'toast-container';
            container.setAttribute('aria-live', 'polite');
            container.setAttribute('aria-atomic', 'false');
            container.style.position = 'fixed';
            container.style.top = '1rem';
            container.style.right = '1rem';
            container.style.zIndex = '1080';
            container.style.maxWidth = '360px';
            document.body.appendChild(container);
        }
        return container;
    }

    function removeToast(wrapper) {
        if (wrapper && wrapper.parentNode) wrapper.parentNode.removeChild(wrapper);
    }

    function createToastElement(type, message, title) {
        const bgClass = type === 'success' ? 'bg-success text-white' : (type === 'error' ? 'bg-danger text-white' : (type === 'warning' ? 'bg-warning text-dark' : 'bg-secondary text-white'));
        const icon = type === 'success' ? 'bi-check-circle' : (type === 'error' ? 'bi-x-circle' : (type === 'warning' ? 'bi-exclamation-triangle' : 'bi-info-circle'));

        const wrapper = document.createElement('div');
        wrapper.className = 'toast align-items-center show';
        wrapper.setAttribute('role', 'status');
        wrapper.setAttribute('aria-live', 'polite');
        wrapper.setAttribute('aria-atomic', 'true');
        wrapper.style.minWidth = '280px';
        wrapper.style.marginBottom = '0.5rem';
        wrapper.tabIndex = 0; // make focusable for screen readers/keyboard

        wrapper.innerHTML = `
            <div class="d-flex ${bgClass} rounded-3 p-2">
                <div class="toast-body d-flex align-items-center" style="flex:1">
                    <i class="bi ${icon} me-2" aria-hidden="true"></i>
                    <div>
                        ${title ? ('<div class="fw-bold">' + title + '</div>') : ''}
                        <div>${message}</div>
                    </div>
                </div>
                <button type="button" class="btn-close btn-close-white ms-2 me-1" aria-label="Close notification"></button>
            </div>
        `;

        return wrapper;
    }

    function showToast(type, message, title) {
        const container = ensureContainer();
        const wrapper = createToastElement(type, message, title);

        container.appendChild(wrapper);

        // Focus for assistive tech
        try { wrapper.focus(); } catch (e) { /* ignore */ }

        const closeBtn = wrapper.querySelector('.btn-close');
        function handleClose() { removeToast(wrapper); window.removeEventListener('keydown', onKeyDown); }
        closeBtn.addEventListener('click', handleClose);

        // Allow keyboard dismiss (Escape)
        function onKeyDown(e) { if (e.key === 'Escape' || e.key === 'Esc') { handleClose(); } }
        window.addEventListener('keydown', onKeyDown);

        // Pause removal on hover to improve accessibility
        let timeoutId = setTimeout(handleClose, 4500);
        wrapper.addEventListener('mouseover', function () { clearTimeout(timeoutId); });
        wrapper.addEventListener('focusin', function () { clearTimeout(timeoutId); });
        wrapper.addEventListener('mouseout', function () { timeoutId = setTimeout(handleClose, 2000); });

        // Ensure close btn is reachable by keyboard
        closeBtn.tabIndex = 0;
        closeBtn.addEventListener('keydown', function (e) { if (e.key === 'Enter' || e.key === ' ') { e.preventDefault(); handleClose(); } });
    }

    window.Toast = {
        show: showToast
    };
})(window);
