import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../base-service';
import { API_ENDPOINTS, environment } from '../../../envoironments/envoironment.dev';
import { AdvertisementCategory } from '../../models/pakClassified/advertisement-category-model';

@Injectable({
  providedIn: 'root'
})
export class AdvertisementCategoryService extends BaseService<AdvertisementCategory> {

  constructor(http: HttpClient) {
    super(http, `${environment.apiUrl}/${API_ENDPOINTS.AdvertisementCategory}`);
  }
}
