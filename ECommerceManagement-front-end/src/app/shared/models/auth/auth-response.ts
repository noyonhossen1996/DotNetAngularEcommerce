export interface AuthResponse {
  userId: string;
  fullName: string;
  email: string;
  role: string;
  accessToken: string;
  refreshToken: string;
}