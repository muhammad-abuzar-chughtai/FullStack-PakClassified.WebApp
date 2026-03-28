import { HttpClient } from "@angular/common/http";
import { Injectable, model } from "@angular/core";
import { API_ENDPOINTS, environment } from '../../../envoironments/envoironment.dev';
import { AdvertisementSearchFilter } from "../../models/pakClassified/advertisement-search-filter-model";
import { Observable } from "rxjs";
import { Advertisement } from "../../models/pakClassified/advertisement-model";


@Injectable({
    providedIn: 'root'
})

export class SearchService {

    private baseUrl = `${environment.apiUrl}/${API_ENDPOINTS.Advertisement}`
    constructor(private http: HttpClient) { }

    Search(model: AdvertisementSearchFilter): Observable<Advertisement[]> {
        return this.http.post<Advertisement[]>(`${this.baseUrl}/filter`, model);
    }
}