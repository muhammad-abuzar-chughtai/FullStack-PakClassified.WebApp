import { inject } from "@angular/core";
import { AuthService } from "../../services/auth/auth-service";
import { CanActivateFn, Router } from "@angular/router";

export const roleGuard: CanActivateFn = (route) => {

  const auth = inject(AuthService);
  const router = inject(Router);

  const allowedRoles = route.data['roleId'] as number[];

  if (!auth.isAuthenticated()) {
    return router.createUrlTree(['/login']);
  }

  if (allowedRoles && !allowedRoles.includes(auth.roleId()!)) {
    return router.createUrlTree(['/unauthorized']);
  }

  return true;
};
