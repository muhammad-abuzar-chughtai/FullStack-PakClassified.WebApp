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

  constructor(private http: HttpClient) { }

  getAll(): Observable<UserGet[]> {
    return this.http.get<UserGet[]>(this.baseUrl);
  }

  getById(id: number): Observable<UserGet> {
    return this.http.get<UserGet>(`${this.baseUrl}/${id}`);
  }

  create(user: UserPost): Observable<UserGet> {
    const formData = new FormData();

    // Required fields — append directly
    formData.append('id', '0');
    formData.append('name', user.name);
    formData.append('email', user.email);
    formData.append('pass', user.password);
    formData.append('profilePic', user.profilePic);
    formData.append('contactNo', user.contactNo.toString());
    formData.append('dob', typeof user.dob === 'string' ? user.dob : user.dob.toISOString());
    formData.append('createdBy', user.createdBy);
    formData.append('roleId', user.roleId.toString());
    // Optional fields — only append if they have a value
    this.appendIfExists(formData, 'secQues', user.secQues);
    this.appendIfExists(formData, 'secAns', user.secAns);

    return this.http.post<UserGet>(this.baseUrl, formData);
  }

  update(id: number, user: UserPost): Observable<UserGet> {
    const formData = new FormData();

    // Required fields
    formData.append('name', user.name);
    formData.append('email', user.email);
    formData.append('contactNo', user.contactNo.toString());
    formData.append('dob', typeof user.dob === 'string' ? user.dob : user.dob.toISOString());
    formData.append('createdBy', user.createdBy);
    formData.append('roleId', user.roleId.toString());
    // Optional fields
    formData.append('profilePic', user.profilePic);
    this.appendIfExists(formData, 'secQues', user.secQues);
    this.appendIfExists(formData, 'secAns', user.secAns);
    this.appendIfExists(formData, 'lastModifiedBy', user.lastmodifiedBy);

      formData.forEach((value, key) => console.log(key, value));

    return this.http.put<UserGet>(`${this.baseUrl}/${id}`, formData);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  private appendIfExists(formData: FormData, key: string, value: any): void {
    if (value !== null && value !== undefined && value !== '') {
      formData.append(key, value);
    }
  }

}
