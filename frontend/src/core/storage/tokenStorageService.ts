import { LoginResponse } from "../../features/auth/types/authTypes.js";

export class TokenStorageService {
  private static readonly TOKEN_KEY = "auth_token";
  private static readonly REFRESH_TOKEN_KEY = "refresh_token";
  private static readonly USER_ID_KEY = "user_id";
  private static readonly EXPIRATION_KEY = "token_expiration";

  public static saveAuthData(loginResponse: LoginResponse): void {
    localStorage.setItem(this.TOKEN_KEY, loginResponse.token);
    localStorage.setItem(this.REFRESH_TOKEN_KEY, loginResponse.refreshToken);
    localStorage.setItem(this.USER_ID_KEY, loginResponse.userId.toString());
    localStorage.setItem(this.EXPIRATION_KEY, loginResponse.expiration);
  }

  public static getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  public static getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  public static getUserId(): number | null {
    const userId = localStorage.getItem(this.USER_ID_KEY);
    return userId ? parseInt(userId, 10) : null;
  }

  public static getExpiration(): string | null {
    return localStorage.getItem(this.EXPIRATION_KEY);
  }

  public static clearAuthData(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
    localStorage.removeItem(this.USER_ID_KEY);
    localStorage.removeItem(this.EXPIRATION_KEY);
  }

  public static isAuthenticated(): boolean {
    return this.getToken() !== null;
  }
}

