import { LoginRequest, LoginResponse } from "../types/authTypes.js";
import { AuthError } from "../types/authError.js";

export class AuthService {
  private static readonly LOGIN_ENDPOINT = "https://localhost:7230/Authentication/Login";
  private static readonly LOGOUT_ENDPOINT = "https://localhost:7230/Authentication/RevokeAccess";

  public static async login(credentials: LoginRequest): Promise<LoginResponse> {
    try {
      const response = await fetch(this.LOGIN_ENDPOINT, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(credentials),
      });

      if (!response.ok) {
        let serverMessage = '';
        try {
          serverMessage = await response.text();
        } catch {
          serverMessage = 'Unknown server error.';
        }

        throw new AuthError(
          `Error on login: ${response.status}`,
          response.status,
          serverMessage
        );
      }

      const data: LoginResponse = await response.json();
      return data;
    } catch (error) {
      if (error instanceof AuthError) {
        throw error;
      }
      if (error instanceof Error) {
        throw new AuthError(error.message, 0, error.message);
      }
      throw new AuthError("Unknown error on login", 0);
    }
  }

  public static async logout(): Promise<void> {
    try {
      const { TokenStorageService } = await import("../../../core/storage/tokenStorageService.js");
      const token = TokenStorageService.getToken();

      if (!token) {
        // Se não houver token, apenas limpa os dados locais e redireciona
        TokenStorageService.clearAuthData();
        window.location.href = '/index.html';
        return;
      }

      const response = await fetch(this.LOGOUT_ENDPOINT, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Authorization": `Bearer ${token}`,
        },
      });

      // Limpa os dados de autenticação independentemente da resposta
      TokenStorageService.clearAuthData();

      // Redireciona para o login mesmo se a requisição falhar
      window.location.href = '/index.html';

    } catch (error) {
      // Em caso de erro, ainda limpa os dados locais e redireciona
      console.error('Error during logout:', error);
      const { TokenStorageService } = await import("../../../core/storage/tokenStorageService.js");
      TokenStorageService.clearAuthData();
      window.location.href = '/index.html';
    }
  }
}

