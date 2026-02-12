import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../base-service';
import { API_ENDPOINTS, environment } from '../../../envoironments/envoironment.dev';
import { Country } from '../../models/location/country-model';

@Injectable({
  providedIn: 'root'
})
export class CountryService extends BaseService<Country> {

  constructor(http: HttpClient) {
    super(http, `${environment.apiUrl}/${API_ENDPOINTS.Country}`);
  }
}
