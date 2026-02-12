import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../base-service';
import { API_ENDPOINTS, environment } from '../../../envoironments/envoironment.dev';
import { CityArea } from '../../models/location/cityarea-model';

@Injectable({
  providedIn: 'root'
})
export class CityAreaService extends BaseService<CityArea> {

  constructor(http: HttpClient) {
    super(http, `${environment.apiUrl}/${API_ENDPOINTS.CityArea}`);
  }
}
