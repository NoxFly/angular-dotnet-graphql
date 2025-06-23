/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { Injectable } from '@angular/core';
import { CanActivate, GuardResult, MaybeAsync, Router } from '@angular/router';
import { filter, map } from 'rxjs';
import { AuthService } from 'src/app/core/services/Auth.service';

@Injectable({
    providedIn: 'root'
})
export class GuestGuard implements CanActivate {
    constructor(
        private readonly authService: AuthService,
        private readonly router: Router
    ) {}

    public canActivate(): MaybeAsync<GuardResult> {
        return this.authService.isAuthenticated$.pipe(
            filter((isAuthenticated) => isAuthenticated !== null),
            map((isAuthenticated: boolean) => {
                if(isAuthenticated)
                    this.router.navigate(['/dashboard']);

                return !isAuthenticated;
            }),
        );
    }
}
