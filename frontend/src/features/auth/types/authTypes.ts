export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  userId: number;
  token: string;
  refreshToken: string;
  expiration: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName?: string;
  email: string;
  password: string;
}

export interface RegisterResponse {
  userId: number;
  token: string;
  refreshToken: string;
  expiration: string;
}

