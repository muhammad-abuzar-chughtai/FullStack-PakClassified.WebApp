import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment, API_ENDPOINTS } from '../../../envoironments/envoironment.dev';
import { SignUp } from '../../models/auth/signup-model';
import { Signin } from '../../models/auth/signin-model';
import { AuthResponse } from '../../models/auth/authresponse-model';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = `${environment.apiUrl}/${API_ENDPOINTS.Auth}`;

  constructor(private http: HttpClient) {}

  signup(user: SignUp): Observable<AuthResponse> {
    const formData = new FormData();
    formData.append('name', user.name);
    formData.append('email', user.email);
    formData.append('password', user.password);
    formData.append('profilePic', user.profilePic);
    formData.append('contactNo', user.contactNo.toString());
    formData.append('dob', user.dob.toISOString());
    formData.append('secQuestion', user.secQuestion);
    formData.append('secAnswer', user.secAnswer);
    formData.append('createdBy', user.createdBy);
    formData.append('lastmodifiedBy', user.lastmodifiedBy);

    return this.http.post<AuthResponse>(`${this.baseUrl}/signup`, formData);
  }

  signin(credentials: Signin): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/signin`, credentials);
  }

}
