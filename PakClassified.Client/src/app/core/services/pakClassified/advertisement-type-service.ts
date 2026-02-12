import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../base-service';
import { API_ENDPOINTS, environment } from '../../../envoironments/envoironment.dev';
import { AdvertisementType } from '../../models/pakClassified/advertisement-type-model';

@Injectable({
    providedIn: 'root'
})
export class AdvertisementTypeService extends BaseService<AdvertisementType> {

    constructor(http: HttpClient) {
        super(http, `${environment.apiUrl}/${API_ENDPOINTS.AdvertisementType}`);
    }
}
