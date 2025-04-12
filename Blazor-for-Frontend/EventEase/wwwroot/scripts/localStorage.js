// Handle storage events to sync user status across tabs
window.setupStorageListener = (dotnetHelper) => {
    window.addEventListener('storage', (event) => {
        if (event.key === 'currentUser') {
            dotnetHelper.invokeMethodAsync('OnStorageChanged');
        }
    });
};