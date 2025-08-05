import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError, catchError, switchMap } from 'rxjs';
import { AuthService } from '../../auth/services/auth.service';
import { Router } from '@angular/router';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService, private router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getAccessToken();
    if (token) {
      request = request.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      });

    }
    
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          return this.handle401Error(request, next);
        }
        return throwError(() => error);
      })
    );
  }

  
  private handle401Error(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.authService.refreshToken().pipe(
      switchMap(response => {
        if (response.success && response.data?.token) {
          this.authService.setAccessToken(response.data.token);

          // Retrying the failed request
          const newRequest = request.clone({
            setHeaders: { Authorization: `Bearer ${response.data.token}` }
          });

          return next.handle(newRequest);
        } else {
          this.authService.logout();
          this.router.navigate(['/auth/login']);
          return throwError(() => new Error('Session expired.'));
        }
      }),
      catchError(() => {
        this.authService.logout();
        this.router.navigate(['/auth/login']);
        return throwError(() => new Error('Session expired.'));
      })
    );
  }
}
