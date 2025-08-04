import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  loginForm: FormGroup;
  hidePassword = true;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

  goToRegister(){
    this.router.navigate(['/auth/register']);
  }

  onSubmit() {
    if (this.loginForm.invalid) return;
    this.isSubmitting = true;

    this.authService.login(this.loginForm.value).subscribe({
      next: (res) => {
        if (res.success) {
          this.successMessage = 'Login successful!';
          this.errorMessage = '';
          this.isSubmitting = false;
          this.router.navigate(['/']);
        } else {
          this.errorMessage = 'Login failed. Please try again.';
          this.successMessage = '';
          this.isSubmitting = false;
        }
      },
      error: (err) => {
        this.errorMessage = 'Login failed. Please try again.';
        this.isSubmitting = false;
      },
    });
  }
}
