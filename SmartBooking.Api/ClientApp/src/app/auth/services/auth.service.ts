import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { LoginRequest } from '../models/login-request.model';
import { RegisterRequest } from '../models/register-request.model';
import { AuthResponse } from '../models/auth-response.model';
import { ApiResponse } from '../../shared/models/api-response.model';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = `${environment.apiBaseUrl}/auth`;

  private isLoggedInSubject = new BehaviorSubject<boolean>(false);

  public isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) {
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

  isAdmin() {
    const token = localStorage.getItem('token');
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      return decodedToken.role === 'Admin';
    } else {
      return false;
    }
  }

  getRole() {
    const token = localStorage.getItem('token');
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      return decodedToken.role;
    } else {
      return null;
    }
  }

  logout() {
    localStorage.removeItem('token');
    this.isLoggedInSubject.next(false);
  }
}
