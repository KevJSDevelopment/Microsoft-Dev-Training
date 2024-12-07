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

// Project Filtering
function initializeProjectFilters() {
    const projects = document.querySelectorAll('.project');
    const filterButtons = document.querySelectorAll('.filter-btn');

    if (filterButtons.length > 0) {
        filterButtons.forEach(button => {
            button.addEventListener('click', () => {
                const category = button.dataset.category;
                filterProjects(category, projects);
                
                // Update active filter button
                filterButtons.forEach(btn => btn.classList.remove('active'));
                button.classList.add('active');
            });
        });
    }
}

function filterProjects(category, projects) {
    projects.forEach(project => {
        const projectCategory = project.dataset.category;
        if (category === 'all' || projectCategory === category) {
            project.style.display = 'block';
            // Add fade-in animation
            project.style.opacity = '0';
            setTimeout(() => {
                project.style.opacity = '1';
            }, 50);
        } else {
            project.style.display = 'none';
        }
    });
}

// Image Lightbox
function initializeImageLightbox() {
    const projectImages = document.querySelectorAll('.project img');
    
    // Create lightbox elements
    const lightbox = document.createElement('div');
    lightbox.className = 'lightbox';
    lightbox.innerHTML = `
        <div class="lightbox-content">
            <img src="" alt="" class="lightbox-image">
            <button class="lightbox-close" aria-label="Close lightbox">×</button>
        </div>
    `;
    document.body.appendChild(lightbox);

    // Set up click handlers
    projectImages.forEach(image => {
        image.addEventListener('click', () => {
            const lightboxImg = lightbox.querySelector('.lightbox-image');
            lightboxImg.src = image.src;
            lightboxImg.alt = image.alt;
            lightbox.style.display = 'flex';
            document.body.style.overflow = 'hidden';
        });
    });

    // Close lightbox
    lightbox.addEventListener('click', (e) => {
        if (e.target === lightbox || e.target.classList.contains('lightbox-close')) {
            lightbox.style.display = 'none';
            document.body.style.overflow = '';
        }
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