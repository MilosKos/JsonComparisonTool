// File download functionality
window.downloadFile = (filename, content, contentType) => {
    // Convert base64 to bytes
    const bytes = atob(content);
    const buffer = new ArrayBuffer(bytes.length);
    const view = new Uint8Array(buffer);
    
    for (let i = 0; i < bytes.length; i++) {
        view[i] = bytes.charCodeAt(i);
    }
    
    // Create blob and download
    const blob = new Blob([buffer], { type: contentType });
    const url = URL.createObjectURL(blob);
    
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
};

// PWA install prompt
let deferredPrompt;

window.addEventListener('beforeinstallprompt', (e) => {
    console.log('beforeinstallprompt event fired');
    // Prevent the mini-infobar from appearing on mobile
    e.preventDefault();
    // Stash the event so it can be triggered later
    deferredPrompt = e;
    
    // Show custom install button
    showInstallButton();
});

function showInstallButton() {
    console.log('Attempting to show install button');
    // Wait for DOM to be ready, then try multiple times if needed
    const tryShowButton = () => {
        const installButton = document.getElementById('install-button');
        if (installButton) {
            console.log('Install button found, making it visible');
            installButton.style.display = 'flex';
            installButton.addEventListener('click', installApp);
            return true;
        }
        return false;
    };
    
    // Try immediately
    if (!tryShowButton()) {
        // If not found, wait and try again (Blazor might still be rendering)
        setTimeout(() => {
            if (!tryShowButton()) {
                console.log('Install button still not found after delay');
            }
        }, 500);
    }
}

// Check if app is already installed
function checkInstallStatus() {
    if (window.navigator.standalone || window.matchMedia('(display-mode: standalone)').matches) {
        console.log('App is already installed');
        return true;
    }
    return false;
}

function installApp() {
    console.log('Install button clicked');
    
    // Hide the install button
    const installButton = document.getElementById('install-button');
    if (installButton) {
        installButton.style.display = 'none';
    }
    
    // Show the install prompt
    if (deferredPrompt) {
        console.log('Showing install prompt');
        deferredPrompt.prompt();
        // Wait for the user to respond to the prompt
        deferredPrompt.userChoice.then((choiceResult) => {
            if (choiceResult.outcome === 'accepted') {
                console.log('User accepted the install prompt');
            } else {
                console.log('User dismissed the install prompt');
                // Show button again if user dismissed
                if (installButton) {
                    installButton.style.display = 'flex';
                }
            }
            deferredPrompt = null;
        });
    } else {
        console.log('Install prompt not available');
        if (installButton) {
            installButton.style.display = 'flex';
        }
        alert('Install not available. Please:\n• Wait 30+ seconds for install prompt\n• Use Chrome/Edge browser\n• Ensure HTTPS connection');
    }
}

// Theme handling
function applyTheme(theme) {
    document.documentElement.setAttribute('data-theme', theme);
    localStorage.setItem('theme', theme);
}

function getPreferredTheme() {
    const storedTheme = localStorage.getItem('theme');
    if (storedTheme) {
        return storedTheme;
    }
    return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
}

// Apply theme on page load
document.addEventListener('DOMContentLoaded', () => {
    applyTheme(getPreferredTheme());
    
    // Add fallback button show for testing (if install prompt hasn't fired after 3 seconds)
    setTimeout(() => {
        if (!deferredPrompt) {
            const installButton = document.getElementById('install-button');
            if (installButton && installButton.style.display === 'none') {
                console.log('Showing install button for testing (no prompt received)');
                installButton.style.display = 'flex';
                installButton.addEventListener('click', installApp);
            }
        }
    }, 3000);
});

// Listen for theme changes
window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', (e) => {
    if (!localStorage.getItem('theme')) {
        applyTheme(e.matches ? 'dark' : 'light');
    }
});
