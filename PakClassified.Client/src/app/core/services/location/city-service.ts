import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../base-service';
import { City } from '../../models/location/city-model';
import { API_ENDPOINTS, environment } from '../../../envoironments/envoironment.dev';

@Injectable({
  providedIn: 'root'
})
export class CityService extends BaseService<City> {

  constructor(http: HttpClient) {
    super(http, `${environment.apiUrl}/${API_ENDPOINTS.City}`);
  }
}
