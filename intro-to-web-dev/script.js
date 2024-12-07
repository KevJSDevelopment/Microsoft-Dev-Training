// Wait for DOM to fully load
document.addEventListener('DOMContentLoaded', () => {
    initializeNavigation();
    initializeSmoothScroll();
    initializeProjectFilters();
    initializeImageLightbox();
    initializeFormValidation();
});

// Navigation Menu Toggle
function initializeNavigation() {
    const hamburger = document.querySelector('.hamburger');
    const navMenu = document.querySelector('nav ul');

    if (hamburger) {
        hamburger.addEventListener('click', () => {
            navMenu.classList.toggle('active');
            hamburger.setAttribute('aria-expanded', 
                hamburger.getAttribute('aria-expanded') === 'false' ? 'true' : 'false'
            );
        });
    }
}

// Smooth Scrolling
function initializeSmoothScroll() {
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function(e) {
            e.preventDefault();
            const targetId = this.getAttribute('href');
            const targetElement = document.querySelector(targetId);
            
            if (targetElement) {
                targetElement.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });

                // Close mobile menu if open
                const navMenu = document.querySelector('nav ul');
                navMenu.classList.remove('active');
            }
        });
    });
}

// Form Validation
function initializeFormValidation() {
    const form = document.querySelector('form');
    const inputs = form.querySelectorAll('input, textarea');

    // Real-time validation
    inputs.forEach(input => {
        input.addEventListener('input', () => {
            validateInput(input);
        });

        input.addEventListener('blur', () => {
            validateInput(input);
        });
    });

    // Form submission
    form.addEventListener('submit', (e) => {
        e.preventDefault();
        
        let isValid = true;
        inputs.forEach(input => {
            if (!validateInput(input)) {
                isValid = false;
            }
        });

        if (isValid) {
            // Show success message
            showFormMessage('Message sent successfully!', 'success');
            form.reset();
        } else {
            showFormMessage('Please fill in all required fields correctly.', 'error');
        }
    });
}

function validateInput(input) {
    const value = input.value.trim();
    let isValid = true;
    let errorMessage = '';

    // Remove existing error message
    const existingError = input.parentElement.querySelector('.error-message');
    if (existingError) {
        existingError.remove();
    }

    // Validation rules
    if (input.required && !value) {
        isValid = false;
        errorMessage = 'This field is required';
    } else if (input.type === 'email' && value) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(value)) {
            isValid = false;
            errorMessage = 'Please enter a valid email address';
        }
    }

    // Show error message if invalid
    if (!isValid) {
        const errorDiv = document.createElement('div');
        errorDiv.className = 'error-message';
        errorDiv.textContent = errorMessage;
        input.parentElement.appendChild(errorDiv);
        input.classList.add('invalid');
    } else {
        input.classList.remove('invalid');
    }

    return isValid;
}

function showFormMessage(message, type) {
    const messageDiv = document.createElement('div');
    messageDiv.className = `form-message ${type}`;
    messageDiv.textContent = message;

    const form = document.querySelector('form');
    const existingMessage = form.querySelector('.form-message');
    if (existingMessage) {
        existingMessage.remove();
    }

    form.insertBefore(messageDiv, form.firstChild);

    // Remove message after 5 seconds
    setTimeout(() => {
        messageDiv.remove();
    }, 5000);
}

// Debug utility function
function debugLog(message, data = null) {
    if (process.env.NODE_ENV !== 'production') {
        console.log(`Debug: ${message}`, data || '');
    }
}