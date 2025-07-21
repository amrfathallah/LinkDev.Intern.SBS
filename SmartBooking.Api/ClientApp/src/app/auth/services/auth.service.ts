import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginRequest } from '../models/login-request.model';
import { RegisterRequest } from '../models/register-request.model';
import { AuthResponse } from '../models/auth-response.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = 'https://localhost:7191/api/auth';

  constructor(private http: HttpClient) {}

  login(data: LoginRequest): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.baseUrl}/login`, data);
  }

  register(data: RegisterRequest): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.baseUrl}/register`, data);
  }
}
