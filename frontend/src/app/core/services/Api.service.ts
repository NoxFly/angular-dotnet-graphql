/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Query } from 'src/app/core/models/misc';
import { environment } from 'src/environments/environment';


@Injectable({
    providedIn: 'root'
})
export class ApiService {
    constructor(
        private readonly http: HttpClient,
    ) {}

    public get$<T>(url: string, query?: Query): Observable<T> {
        return this.http.get<T>(environment.apiUrl + url, { params: query });
    }

    public post$<T>(url: string, body?: unknown, query?: Query): Observable<T> {
        return this.http.post<T>(environment.apiUrl + url, body, { params: query });
    }

    public put$<T>(url: string, body?: unknown, query?: Query): Observable<T> {
        return this.http.put<T>(environment.apiUrl + url, body, { params: query });
    }

    public patch$<T>(url: string, body?: unknown, query?: Query): Observable<T> {
        return this.http.patch<T>(environment.apiUrl + url, body, { params: query });
    }

    public delete$<T>(url: string): Observable<T> {
        return this.http.delete<T>(environment.apiUrl + url);
    }
}
