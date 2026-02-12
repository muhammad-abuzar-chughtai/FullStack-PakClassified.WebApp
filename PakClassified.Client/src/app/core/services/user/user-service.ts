import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment, API_ENDPOINTS } from '../../../envoironments/envoironment.dev';
import { UserGet, UserPost } from '../../models/user/user-model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseUrl = `${environment.apiUrl}/${API_ENDPOINTS.User}`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<UserGet[]> {
    return this.http.get<UserGet[]>(this.baseUrl);
  }

  getById(id: number): Observable<UserGet> {
    return this.http.get<UserGet>(`${this.baseUrl}/${id}`);
  }

  create(user: UserPost): Observable<UserGet> {
    const formData = new FormData();

    formData.append('id', user.id.toString());
    formData.append('name', user.name);
    formData.append('email', user.email);
    formData.append('profilePic', user.profilePic); // File
    formData.append('contactNo', user.contactNo.toString());
    formData.append('dob', user.dob.toISOString());
    formData.append('secQuestion', user.secQuestion);
    formData.append('secAnswer', user.secAnswer);
    formData.append('createdBy', user.createdBy);
    formData.append('lastmodifiedBy', user.lastmodifiedBy);
    formData.append('roleId', user.roleId.toString());

    return this.http.post<UserGet>(this.baseUrl, formData);
  }

  update(id: number, user: UserPost): Observable<UserGet> {
    const formData = new FormData();

    formData.append('name', user.name);
    formData.append('email', user.email);
    if (user.profilePic) {
      formData.append('profilePic', user.profilePic);
    }
    formData.append('contactNo', user.contactNo.toString());
    formData.append('dob', user.dob.toISOString());
    formData.append('secQuestion', user.secQuestion);
    formData.append('secAnswer', user.secAnswer);
    formData.append('lastmodifiedBy', user.lastmodifiedBy);
    formData.append('roleId', user.roleId.toString());

    return this.http.put<UserGet>(`${this.baseUrl}/${id}`, formData);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

}
