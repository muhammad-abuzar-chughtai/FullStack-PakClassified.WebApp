import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../../services/auth/auth-service';

// export const authInterceptor: HttpInterceptorFn = (req, next) => {

//   const auth = inject(AuthService);
//   const token = auth.token(); // signal

//   if (req.url.includes('/signin') || req.url.includes('/signup')) {
//     return next(req);
//     }

//   if (token) {
//     req = req.clone({
//       setHeaders: {
//         Authorization: `Bearer ${token}`
//       }
//     });
//   }

//   return next(req);
// };
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

  return next(req);
};

