import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth/services/auth.service';
@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
})
export class NavMenuComponent implements OnInit {
  isLoggedIn = false;
  isAdmin = false;
  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit(): void {
    debugger;
    this.checkLoginStatus();
  }

  checkLoginStatus() {
    this.authService.isLoggedIn$.subscribe((isLoggedIn) => {
      debugger;
      this.isLoggedIn = isLoggedIn;
      this.isAdmin = this.authService.isAdmin();
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
