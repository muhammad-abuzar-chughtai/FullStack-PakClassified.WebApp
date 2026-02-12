import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../base-service';
import { API_ENDPOINTS, environment } from '../../../envoironments/envoironment.dev';
import { AdvertisementStatus } from '../../models/pakClassified/advertisement-status-model';

@Injectable({
    providedIn: 'root'
})
export class AdvertisementStatusService extends BaseService<AdvertisementStatus> {

    constructor(http: HttpClient) {
        super(http, `${environment.apiUrl}/${API_ENDPOINTS.AdvertisementStatus}`);
    }
}
