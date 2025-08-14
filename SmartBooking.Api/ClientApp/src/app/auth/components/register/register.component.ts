import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { RegisterRequest } from '../../models/register-request.model';
import { ApiResponse } from 'src/app/shared/models/api-response.model';
import { AuthResponse } from '../../models/auth-response.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registrationForm: FormGroup;
  isSubmitting = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registrationForm = this.fb.group({
      fullName: ['', Validators.required],
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirm = form.get('confirmPassword')?.value;
    return password === confirm ? null : { mismatch: true };
  }

  goToLogin() {
    this.router.navigate(['/auth/login']);
  }

  onSubmit() {
    debugger;
    if (this.registrationForm.invalid) {
      this.registrationForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';


    const { fullName, userName, email, password } = this.registrationForm.value;
    const registerData: RegisterRequest = { fullName, userName, email, password };

    this.authService.register(registerData).subscribe({
      next: (response: ApiResponse<AuthResponse>) => {
        if (response.success) {
          alert('Registration successful!');
          this.router.navigate(['/auth/login']);
          this.isSubmitting = false;
          this.registrationForm.reset();
        }else{
          this.errorMessage = response.message;
        this.isSubmitting = false;
        }
        

      },
      error: err => {
        this.errorMessage = "";
        this.isSubmitting = false;
      }
    });
  }
}
