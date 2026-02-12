import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../base-service';
import { API_ENDPOINTS, environment } from '../../../envoironments/envoironment.dev';
import { AdvertisementSubCategory } from '../../models/pakClassified/advertisement-subcategory-model';
@Injectable({
  providedIn: 'root'
})
export class AdvertisementSubCategoryService extends BaseService<AdvertisementSubCategory> {

  constructor(http: HttpClient) {
    super(http, `${environment.apiUrl}/${API_ENDPOINTS.AdvertisementSubCategory}`);
  }
}
