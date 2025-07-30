import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './auth/services/auth.service';
import { catchError, map, Observable, of } from 'rxjs';


@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(): Observable<boolean> {
    const isLoggedIn = this.authService.isLoggedIn;
    const isTokenExpired = this.authService.isTokenExpired();

    if (isLoggedIn && !isTokenExpired) {
      return of(true);
    }
    else if (isTokenExpired) {
      return this.authService.refreshToken().pipe(
        map(response => {
          return response.success
        }),
        catchError(_ => of(false))
      );
    }
    else{
      this.router.navigate(['/auth/login']);
      return of(false);
    }

  }
}

