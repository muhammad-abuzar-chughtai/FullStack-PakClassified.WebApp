import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../base-service';
import { API_ENDPOINTS, environment } from '../../../envoironments/envoironment.dev';
import { Role } from '../../models/user/role-model';

@Injectable({
    providedIn: 'root'
})
export class RoleService extends BaseService<Role> {

    constructor(http: HttpClient) {
        super(http, `${environment.apiUrl}/${API_ENDPOINTS.Role}`);
    }
}
