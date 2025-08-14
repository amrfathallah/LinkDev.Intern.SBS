import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { LoginRequest } from '../models/login-request.model';
import { RegisterRequest } from '../models/register-request.model';
import { AuthResponse } from '../models/auth-response.model';
import { ApiResponse } from '../../shared/models/api-response.model';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../../environments/environment';
import { Router } from '@angular/router';
import { observableToBeFn } from 'rxjs/internal/testing/TestScheduler';
import { tokenDto } from '../models/refreshToken-request.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = `${environment.apiBaseUrl}/auth`;

  private isLoggedInSubject = new BehaviorSubject<boolean>(false);

  public isLoggedIn$ = this.isLoggedInSubject.asObservable();
  public get isLoggedIn() {
    return this.isLoggedInSubject.value;
  }

  constructor(
    private http: HttpClient,
    private jwtHelper: JwtHelperService,
    private router: Router
  ) {
    const token = localStorage.getItem('token');
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.isLoggedInSubject.next(true);
    } else {
      this.isLoggedInSubject.next(false);
    }
  }

  login(data: LoginRequest): Observable<ApiResponse<AuthResponse>> {
    return this.http
      .post<ApiResponse<AuthResponse>>(`${this.baseUrl}/login`, data, {
        withCredentials: true,
      })
      .pipe(
        map((response: ApiResponse<AuthResponse>) => {
          if (response.success && response.data && response.data.token) {
            localStorage.setItem('token', response.data.token);
            localStorage.setItem('refreshToken', response.data.refreshToken);
            this.isLoggedInSubject.next(true);
          } else {
            this.isLoggedInSubject.next(false);
          }
          return response;
        })
      );
  }

  register(data: RegisterRequest): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(
      `${this.baseUrl}/register`,
      data,
      {
        withCredentials: true,
      }
    );
  }

  setAccessToken(accessToken: string) {
    localStorage.setItem('token', accessToken);
  }

  getAccessToken() {
    return localStorage.getItem('token');
  }

  getRefreshToken() {
    return localStorage.getItem('refreshToken');
  }

  refreshToken(): Observable<ApiResponse<AuthResponse>> {
    const accessToken: string = localStorage.getItem('token') ?? '';
    const refreshToken: string = localStorage.getItem('refreshToken') ?? '';
    var tokenDto: tokenDto = { accessToken, refreshToken };

    return this.http
      .post<ApiResponse<AuthResponse>>(`${this.baseUrl}/refresh`, tokenDto, {
        withCredentials: true,
      })
      .pipe(
        tap((response) => {
          if (response.success) {
            this.setAccessToken(response.data.token);
            this.isLoggedInSubject.next(true);
          }
        })
      );
  }

  isTokenExpired() {
    let token = localStorage.getItem('token');
    return this.jwtHelper.isTokenExpired(token);
  }

  getRole() {
    const token = localStorage.getItem('token');
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      return (
        decodedToken[
          'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
        ] ||
        decodedToken.role ||
        null
      );
    }
    return null;
  }

  isAdmin(): boolean {
    const role = this.getRole();
    return role === 'Admin';
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    this.router.navigate(['/auth/login']);
    this.isLoggedInSubject.next(false);
  }
}
