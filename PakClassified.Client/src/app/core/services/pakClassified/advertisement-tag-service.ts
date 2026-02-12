import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../base-service';
import { API_ENDPOINTS, environment } from '../../../envoironments/envoironment.dev';
import { AdvertisementTag } from '../../models/pakClassified/advertisement-tag-model';

@Injectable({
    providedIn: 'root'
})
export class AdvertisementTagService extends BaseService<AdvertisementTag> {

    constructor(http: HttpClient) {
        super(http, `${environment.apiUrl}/${API_ENDPOINTS.AdvertisementTag}`);
    }
}
