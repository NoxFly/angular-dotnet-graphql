/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, switchMap, catchError, throwError, of } from "rxjs";
import { AuthService } from "src/app/core/services/Auth.service";

@Injectable()
export class RequestInterceptor implements HttpInterceptor {
    constructor(
        private readonly authService: AuthService,
    ) {}

    public intercept(req: HttpRequest<unknown>, handler: HttpHandler): Observable<HttpEvent<unknown>> {
        return this.prepareRequest$(req).pipe(
            switchMap((req) => handler.handle(req)),
            catchError((error: HttpErrorResponse) => this.handleError$(error)),
        );
    }

    private prepareRequest$(req: HttpRequest<unknown>): Observable<HttpRequest<unknown>> {
        let headers = req.headers
            .set("Content-Type", "application/json")
            .set("Accept", "application/json")
            .set("X-Requested-With", "XMLHttpRequest")
            ;

        const bearer = this.authService.getBearer();

        if(bearer) {
            headers = headers.set("Authorization", `Bearer ${bearer.accessToken}`);
        }

        const r = req.clone({
            headers,
            withCredentials: true,
            responseType: "json",
        });

        return of(r);
    }

    private handleError$(error: HttpErrorResponse): Observable<never> {
        if(error.status === 401) {
            return this.authService.logout$().pipe(
                switchMap(() => throwError(() => error))
            );
        }

        return throwError(() => error);
    }
}
