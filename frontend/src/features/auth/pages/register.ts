import { RegisterService } from "../services/registerService.js";
import { AuthError } from "../types/authError.js";

document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('register-form') as HTMLFormElement | null;
    const errorMessageEmail = document.getElementById('error-message-email') as HTMLDivElement | null;
    const errorMessagePassword = document.getElementById('error-message-password') as HTMLDivElement | null;
    const successMessage = document.getElementById('success-message') as HTMLDivElement | null;

    if (!form) {
        console.error('Formulário de registro não encontrado');
        return;
    }

    const showError = (message: string, field: 'email' | 'password'): void => {
        const errorElement = field === 'email' ? errorMessageEmail : errorMessagePassword;
        const inputElement = form?.querySelector<HTMLInputElement>(`#${field}`);

        if (errorElement) {
            errorElement.textContent = message;
            errorElement.style.display = 'block';
        }

        if (inputElement) {
            inputElement.classList.add('error');
        }
    };

    const hideError = (field: 'email' | 'password' | 'all'): void => {
        if (field === 'all' || field === 'email') {
            if (errorMessageEmail) {
                errorMessageEmail.style.display = 'none';
                errorMessageEmail.textContent = '';
            }
            const emailInput = form?.querySelector<HTMLInputElement>('#email');
            if (emailInput) {
                emailInput.classList.remove('error');
            }
        }

        if (field === 'all' || field === 'password') {
            if (errorMessagePassword) {
                errorMessagePassword.style.display = 'none';
                errorMessagePassword.textContent = '';
            }
            const passwordInput = form?.querySelector<HTMLInputElement>('#password');
            if (passwordInput) {
                passwordInput.classList.remove('error');
            }
        }
    };

    const showSuccess = (message: string): void => {
        if (successMessage) {
            successMessage.textContent = message;
            successMessage.style.display = 'flex';
        }

        if (form) {
            form.style.opacity = '0.5';
            form.style.pointerEvents = 'none';
        }
    };

    const hideSuccess = (): void => {
        if (successMessage) {
            successMessage.style.display = 'none';
            successMessage.textContent = '';
        }

        if (form) {
            form.style.opacity = '1';
            form.style.pointerEvents = 'auto';
        }
    };

    const emailInput = form.querySelector<HTMLInputElement>('#email');
    const passwordInput = form.querySelector<HTMLInputElement>('#password');

    emailInput?.addEventListener('input', () => {
        hideError('email');
        hideSuccess();
    });

    passwordInput?.addEventListener('input', () => {
        hideError('password');
        hideSuccess();
    });

    form.addEventListener('submit', async (event) => {
        event.preventDefault();

        hideError('all');
        hideSuccess();

        const firstNameInput = form.querySelector<HTMLInputElement>('#firstName');
        const lastNameInput = form.querySelector<HTMLInputElement>('#lastName');
        const emailInputValue = form.querySelector<HTMLInputElement>('#email');
        const passwordInputValue = form.querySelector<HTMLInputElement>('#password');

        if (!firstNameInput || !emailInputValue || !passwordInputValue) {
            console.error('Campos obrigatórios não encontrados');
            return;
        }

        const firstName = firstNameInput.value.trim();
        const lastName = lastNameInput?.value.trim() || undefined;
        const email = emailInputValue.value.trim();
        const password = passwordInputValue.value;

        if (!firstName || !email || !password) {
            alert('Please fill in all required fields');
            return;
        }

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            showError('Please enter a valid email', 'email');
            return;
        }

        try {
            const registerResponse = await RegisterService.register({
                firstName,
                lastName,
                email,
                password,
            });
            
            showSuccess('Registration successful! Redirecting to login...');
            
            setTimeout(() => {
                window.location.href = '/index.html';
            }, 2000);
            
        } catch (error) {
            console.error('Error on registration:', error);
            
            if (error instanceof AuthError) {
                if (error.status === 400) {

                    const errorMessage = error.serverMessage || error.message;
                    
                    if (errorMessage.toLowerCase().includes('email')) {
                        showError(errorMessage, 'email');
                    } else if (errorMessage.toLowerCase().includes('password')) {
                        showError(errorMessage, 'password');
                    } else if (errorMessage.toLowerCase().includes('first name')) {
                        alert(errorMessage);
                    } else {
                        showError(errorMessage, 'password');
                    }
                } else {
                    alert(error.serverMessage || error.message);
                }
            } else {
                const message = error instanceof Error ? error.message : 'Error registering. Please try again.';
                alert(message);
            }
        }
    });
});

