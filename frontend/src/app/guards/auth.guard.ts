import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const expectedRole = route.data['role'];
  const currentRole = authService.getRole();

  if (!authService.getToken()) {
    return router.createUrlTree(['/login']);
  }

  if (expectedRole && expectedRole !== currentRole) {
    return router.createUrlTree(['/catalog']);
  }

  return true;
};
