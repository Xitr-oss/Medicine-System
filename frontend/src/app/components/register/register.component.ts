import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  fb = inject(FormBuilder);
  authService = inject(AuthService);
  router = inject(Router);

  registerForm: FormGroup;
  error: string = '';
  loading = false;

  constructor() {
    this.registerForm = this.fb.group({
      name: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern('^[0-9]+$')]], // Added basic validation for phone digits
      address: ['', [Validators.required]]
    });
  }

  onSubmit() {
    if (this.registerForm.invalid) return;

    this.loading = true;
    this.error = '';

    this.authService.register(this.registerForm.value).subscribe({
      next: () => {
        this.loading = false;
        // Auto-login or redirect to login. Usually redirect is safer.
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || 'Registration failed. Check details.';
      }
    });
  }
}
