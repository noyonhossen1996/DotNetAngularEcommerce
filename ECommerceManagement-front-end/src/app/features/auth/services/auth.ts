import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, tap } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../shared/models/api-response.model';
import { LoginRequest } from '../../../shared/models/auth/login-request';
import { RegisterRequest } from '../../../shared/models/auth/register-request';
import { AuthResponse } from '../../../shared/models/auth/auth-response';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiBaseUrl}/auth`;

  currentUser = signal<AuthResponse | null>(this.getStoredUser());

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<ApiResponse<AuthResponse>>(`${this.apiUrl}/login`, request)
      .pipe(
        map(response => response.data),
        tap(user => this.storeUser(user))
      );
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<ApiResponse<AuthResponse>>(`${this.apiUrl}/register`, request)
      .pipe(
        map(response => response.data),
        tap(user => this.storeUser(user))
      );
  }

  logout(): void {
    localStorage.removeItem('auth_user');
    this.currentUser.set(null);
  }

  getAccessToken(): string | null {
    return this.currentUser()?.accessToken ?? null;
  }

  isLoggedIn(): boolean {
    return !!this.getAccessToken();
  }

  isAdmin(): boolean {
    return this.currentUser()?.role === 'Admin';
  }

  private storeUser(user: AuthResponse): void {
    localStorage.setItem('auth_user', JSON.stringify(user));
    this.currentUser.set(user);
  }

  private getStoredUser(): AuthResponse | null {
    const user = localStorage.getItem('auth_user');
    return user ? JSON.parse(user) : null;
  }
}