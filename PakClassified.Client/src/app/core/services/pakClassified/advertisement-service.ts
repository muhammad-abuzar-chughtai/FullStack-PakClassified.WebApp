import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../base-service';
import { API_ENDPOINTS, environment } from '../../../envoironments/envoironment.dev';
import { Advertisement } from '../../models/pakClassified/advertisement-model';

@Injectable({
    providedIn: 'root'
})
export class AdvertisementService extends BaseService<Advertisement> {

    constructor(http: HttpClient) {
        super(http, `${environment.apiUrl}/${API_ENDPOINTS.Advertisement}`);
    }
}
