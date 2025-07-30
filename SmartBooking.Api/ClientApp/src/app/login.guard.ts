import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './auth/services/auth.service';
import { catchError, map, Observable, of } from 'rxjs';


@Injectable({ providedIn: 'root' })
export class LoginGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(): boolean {
    const isLoggedIn = this.authService.isLoggedIn;
    if(isLoggedIn){
      return false;
    }
    return true;
  }
}

