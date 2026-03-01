import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment, API_ENDPOINTS } from '../../../envoironments/envoironment.dev';
import { SignUp } from '../../models/auth/signup-model';
import { Signin } from '../../models/auth/signin-model';
import { AuthResponse } from '../../models/auth/authresponse-model';
import { Router } from '@angular/router';
import { tap } from 'rxjs';

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
  isAuthenticated = computed(() => !!this._token());
  roleId = computed(() => this._user()?.roleId ?? null);

  private baseUrl = `${environment.apiUrl}/${API_ENDPOINTS.Auth}`;

  constructor(private http: HttpClient, private router: Router) {
    this.initialize();
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
    this.router.navigate(['/login']);
  }

  initialize() {
    const token = localStorage.getItem('token');
    const user = localStorage.getItem('user');

    if (token && user) {
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
