import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_ENDPOINTS, environment } from '../../../envoironments/envoironment.dev';
import { AdvertisementImageGet, AdvertisementImagePost } from '../../models/pakClassified/advertisement-image-model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdvertisementImageService {

  private baseUrl = `${environment.apiUrl}/${API_ENDPOINTS.AdvertisementImage}`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<AdvertisementImageGet[]> {
    return this.http.get<AdvertisementImageGet[]>(this.baseUrl);
  }

  getById(id: number): Observable<AdvertisementImageGet> {
    return this.http.get<AdvertisementImageGet>(`${this.baseUrl}/${id}`);
  }

  create(model: AdvertisementImagePost): Observable<AdvertisementImageGet> {

    const formData = new FormData();

    formData.append('id', model.id.toString());
    formData.append('name', model.name);
    formData.append('content', model.content); // File
    formData.append('advertisementId', model.advertisementId.toString());
    formData.append('createdBy', model.createdBy);

    if (model.caption) {
      formData.append('caption', model.caption);
    }

    if (model.lastModifiedBy) {
      formData.append('lastModifiedBy', model.lastModifiedBy);
    }

    return this.http.post<AdvertisementImageGet>(this.baseUrl, formData);
  }

  update(id: number, model: AdvertisementImagePost): Observable<AdvertisementImageGet> {

    const formData = new FormData();

    formData.append('name', model.name);
    formData.append('content', model.content); // File
    formData.append('advertisementId', model.advertisementId.toString());

    if (model.caption) {
      formData.append('caption', model.caption);
    }

    if (model.lastModifiedBy) {
      formData.append('lastModifiedBy', model.lastModifiedBy);
    }

    return this.http.put<AdvertisementImageGet>(
      `${this.baseUrl}/${id}`,
      formData
    );
  }

  delete(id: number): Observable<AdvertisementImageGet> {
    return this.http.delete<AdvertisementImageGet>(`${this.baseUrl}/${id}`);
  }

}
