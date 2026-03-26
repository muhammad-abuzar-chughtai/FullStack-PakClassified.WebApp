import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../../services/auth/auth-service';
import { catchError, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  if (req.url.includes('/signin') || req.url.includes('/signup')) {
    return next(req);
  }

  const auth = inject(AuthService);
  const token = auth.token();

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      // ---- server says token is no longer valid ----
      if (err.status === 401) {
        auth.logout();
      }
      return throwError(() => err);
    })
  );
};

