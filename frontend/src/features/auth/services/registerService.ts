import { RegisterRequest, RegisterResponse } from "../types/authTypes.js";
import { AuthError } from "../types/authError.js";

export class RegisterService {
  private static readonly REGISTER_ENDPOINT = "https://localhost:7230/Authentication/Register";

  public static async register(userData: RegisterRequest): Promise<void> {
    try {
      const response = await fetch(this.REGISTER_ENDPOINT, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          firstName: userData.firstName,
          lastName: userData.lastName || null,
          email: userData.email,
          password: userData.password,
        }),
      });

      if (!response.ok) {
        let serverMessage = '';
        try {
          serverMessage = await response.text();
        } catch {
          serverMessage = 'Unknown server error.';
        }

        throw new AuthError(
          `Error on registration: ${response.status}`,
          response.status,
          serverMessage
        );
      }

    } catch (error) {
      if (error instanceof AuthError) {
        throw error;
      }
      if (error instanceof Error) {
        throw new AuthError(error.message, 0, error.message);
      }
      throw new AuthError("Unknown error on registration", 0);
    }
  }
}

