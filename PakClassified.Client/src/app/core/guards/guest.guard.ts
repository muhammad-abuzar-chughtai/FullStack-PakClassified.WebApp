import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth/auth-service';

export const guestGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  // already logged in → skip login, go to dashboard
  if (auth.isAuthenticated()) {
    return router.createUrlTree(['/admin']);
  }

  return true;
};