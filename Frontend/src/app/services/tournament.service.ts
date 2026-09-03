import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment';
import { Pokemon, SortDirection, SortOptions } from '../models/pokemon.model';

@Injectable({
    providedIn: 'root'
})
export class TournamentService {

    private apiUrl = environment.apiUrl + '/pokemon';

    constructor(private http: HttpClient) {}

    getTournamentStatistics(sortBy: SortOptions, sortDirection: SortDirection): Observable<Pokemon[]> {
        const params = new HttpParams()
            .set('sortBy', sortBy)
            .set('sortDirection', sortDirection);

        return this.http.get<Pokemon[]>(`${this.apiUrl}/tournament/statistics`, { params });
    }

    
}
