import { AuthService } from "../services/authService.js";
import { TokenStorageService } from "../../../core/storage/tokenStorageService.js";
import { AuthError } from "../types/authError.js";

document.addEventListener('DOMContentLoaded', () => {
    const form = document.querySelector('.login-form') as HTMLFormElement | null;
    const errorMessageElement = document.getElementById('error-message') as HTMLDivElement | null;

    if (!errorMessageElement) {
        console.error('Error message element not found');
    }

    const showError = (message: string): void => {
        if (errorMessageElement) {
            errorMessageElement.textContent = message;
            errorMessageElement.style.display = 'block';
        }
        
        const passwordInput = form?.querySelector<HTMLInputElement>('input[type="password"]');
        if (passwordInput) {
            passwordInput.classList.add('error');
        }
    };

    const hideError = (): void => {
        if (errorMessageElement) {
            errorMessageElement.style.display = 'none';
            errorMessageElement.textContent = '';
        }
        
        const passwordInput = form?.querySelector<HTMLInputElement>('input[type="password"]');
        if (passwordInput) {
            passwordInput.classList.remove('error');
        }
    };

    const passwordInput = form?.querySelector<HTMLInputElement>('input[type="password"]');
    passwordInput?.addEventListener('input', () => {
        hideError();
    });

    form?.addEventListener('submit', async (event) => {
        event.preventDefault();

        hideError();

        const emailInput = form.querySelector<HTMLInputElement>('input[type="email"]');
        const passwordInput = form.querySelector<HTMLInputElement>('input[type="password"]');

        if (!emailInput || !passwordInput) {
            console.error('Email or password fields not found');
            return;
        }

        const email = emailInput.value.trim();
        const password = passwordInput.value;

        if (!email || !password) {
            alert('Please fill in all fields');
            return;
        }

        try {
            const loginResponse = await AuthService.login({ email, password });
            
            TokenStorageService.saveAuthData(loginResponse);
            
            window.location.href = '/chat.html';
            
        } catch (error) {
            console.error('Error logging in:', error);
            
            if (error instanceof AuthError && error.status === 400) {
                const message = error.serverMessage || error.message;
                showError(message);
            } else {
                const message = error instanceof Error ? error.message : 'Error logging in. Please try again.';
                alert(message);
            }
        }
    });
});

