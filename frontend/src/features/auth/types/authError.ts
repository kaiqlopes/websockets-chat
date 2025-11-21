export class AuthError extends Error {
  constructor(
    message: string,
    public readonly status: number,
    public readonly serverMessage?: string
  ) {
    super(message);
    this.name = 'AuthError';
  }
}

