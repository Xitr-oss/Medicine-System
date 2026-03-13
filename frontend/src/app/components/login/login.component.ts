import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  fb = inject(FormBuilder);
  authService = inject(AuthService);
  router = inject(Router);

  loginForm: FormGroup;
  error: string = '';
  loading = false;

  constructor() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]] // represents Phone in DB
    });
  }

  onSubmit() {
    if (this.loginForm.invalid) return;
    
    this.loading = true;
    this.error = '';

    this.authService.login(this.loginForm.value).subscribe({
      next: (res) => {
        this.loading = false;
        if (res.role === 'Admin') {
          this.router.navigate(['/admin/dashboard']);
        } else {
          this.router.navigate(['/catalog']);
        }
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || 'Login failed. Please try again.';
      }
    });
  }
}
