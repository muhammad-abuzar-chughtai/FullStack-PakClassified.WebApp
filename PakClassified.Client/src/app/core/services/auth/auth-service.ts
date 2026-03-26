import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment, API_ENDPOINTS } from '../../../envoironments/envoironment.dev';
import { SignUp } from '../../models/auth/signup-model';
import { Signin } from '../../models/auth/signin-model';
import { AuthResponse } from '../../models/auth/authresponse-model';
import { Router } from '@angular/router';
import { map, Observable, tap } from 'rxjs';
import { ForgetPassDto } from '../../models/auth/forgetpass-model';
import { ResetPasswordDto } from '../../models/auth/resetpassword-model';

@Injectable({ 
  providedIn: 'root' 
})
export class AuthService {

  // ---- Core Signals ----
  private _token = signal<string | null>(null);
  private _user = signal<AuthResponse['payload'] | null>(null);

  // ---- Public Signals ----
  token = computed(() => this._token());
  user = computed(() => this._user());
  isAuthenticated = computed(() => this.checkTokenValid());  // ← changed
  roleId = computed(() => this._user()?.roleId ?? null);
  roleName = computed(() => this._user()?.roleName ?? null);

  private baseUrl = `${environment.apiUrl}/${API_ENDPOINTS.Auth}`;

  constructor(private http: HttpClient, private router: Router) {
    this.initialize();
  }

   // ---- NEW: decode JWT and check exp claim ----
  private isTokenExpired(token: string): boolean {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const now = Math.floor(Date.now() / 1000); // seconds
      return payload.exp < now;
    } catch {
      return true; // malformed token = treat as expired
    }
  }

  private checkTokenValid(): boolean {
    const token = this._token();
    if (!token) return false;
    if (this.isTokenExpired(token)) {
      this.logout(); // auto-clear expired session
      return false;
    }
    return true;
  }

  signup(user: SignUp) {
    const formData = this.buildFormData(user);

    return this.http.post<AuthResponse>(`${this.baseUrl}/signup`, formData)
      .pipe(tap(res => this.setSession(res)));
  }

  signin(credentials: Signin) {
    return this.http.post<AuthResponse>(`${this.baseUrl}/signin`, credentials)
      .pipe(tap(res => this.setSession(res)));
  }

  verifySecurity(dto: ForgetPassDto): Observable<string> {
    return this.http
      .post<{ resetToken: string }>(`${this.baseUrl}/verify-security`, dto)
      .pipe(map(res => res.resetToken));
  }

  resetPassword(dto: ResetPasswordDto): Observable<string> {
    return this.http
      .post<{ message: string }>(`${this.baseUrl}/reset-password`, dto)
      .pipe(map(res => res.message));
  }

  private setSession(res: AuthResponse) {
    localStorage.setItem('token', res.token);
    localStorage.setItem('user', JSON.stringify(res.payload));

    this._token.set(res.token);
    this._user.set(res.payload);
  }

  logout() {
    localStorage.clear();
    this._token.set(null);
    this._user.set(null);
    this.router.navigate(['']);
  }

    initialize() {
    const token = localStorage.getItem('token');
    const user = localStorage.getItem('user');

    if (token && user) {
      // ---- only restore session if token is still valid ----
      if (this.isTokenExpired(token)) {
        localStorage.clear(); // wipe expired session immediately
        return;
      }
      this._token.set(token);
      this._user.set(JSON.parse(user));
    }
  }

  private buildFormData(user: SignUp): FormData {
    const formData = new FormData();

    Object.entries(user).forEach(([key, value]) => {
      if (value instanceof Date) {
        formData.append(key, value.toISOString());
      } else {
        formData.append(key, value as any);
      }
    });

    return formData;
  }
}
