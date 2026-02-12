import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../base-service';
import { API_ENDPOINTS, environment } from '../../../envoironments/envoironment.dev';
import { Province } from '../../models/location/province-model';

@Injectable({
  providedIn: 'root'
})
export class ProvinceService extends BaseService<Province> {

  constructor(http: HttpClient) {
    super(http, `${environment.apiUrl}/${API_ENDPOINTS.Province}`);
  }
}
