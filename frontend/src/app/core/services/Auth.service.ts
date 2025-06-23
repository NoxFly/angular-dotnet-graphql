/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { BehaviorSubject, catchError, forkJoin, from, map, Observable, of, switchMap, tap } from 'rxjs';
import { AuthResponse, Bearer, Credentials } from 'src/app/core/models/dto/auth';
import { Maybe } from 'src/app/core/models/misc';
import { ApiService } from 'src/app/core/services/Api.service';
import { ResetUser, SetBearer, SetUser } from 'src/app/core/states/User/User.action';
import { UserState } from 'src/app/core/states/User/User.state';
import { encryptRSA } from 'src/app/shared/helpers/encryption';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private readonly isAuthenticatedSubject = new BehaviorSubject<Maybe<boolean>>(null);
    private bearer: Maybe<Bearer> = null;

    /**
     * @description
     * Navigue vers la page par défaut de l'utilisateur non connecté.
     * La redirection efface la pile de navigation,
     * faisant de cette page la racine, donc pas de retour possible.
     */
    private navigateToDefaultGuestPage$(): Observable<void> {
        return from(this.router.navigate(["/auth/login"], {
            replaceUrl: true,
        })).pipe(map(() => void 0));
    }

    /**
     * @description
     * Navigue vers la page par défaut de l'utilisateur connecté.
     * La redirection efface la pile de navigation,
     * faisant de cette page la racine, donc pas de retour possible.
     */
    private navigateToDefaultUserPage$(): Observable<void> {
        return from(this.router.navigate(["/dashboard"], {
            replaceUrl: true,
        })).pipe(map(() => void 0));
    }

    constructor(
        private readonly api: ApiService,
        private readonly router: Router,
        private readonly store: Store,
    ) {}

    /**
     * @description
     * Booléen de l'état de connectivité de l'utilisateur.
     * Renvoie si état == connecté.
     */
    public get isAuthenticated(): boolean {
        return this.isAuthenticatedSubject.getValue() === true;
    }

    /**
     * @description
     * Observable de l'état de connectivité de l'utilisateur.
     * Renvoie si état == connecté.
     */
    public get isAuthenticated$(): Observable<Maybe<boolean>> {
        return this.isAuthenticatedSubject.asObservable();
    }

    public getBearer(): Maybe<Bearer> {
        return this.bearer;
    }

    /**
     * @description
     * Essaie de connecter l'utilisateur avec les identifiants donnés.
     * Si réussi, redirige vers la page d'accueil de l'utilisateur.
     * Sinon, throw une erreur.
     *
     * @param credentials Les identifiants à utiliser.
    */
    public login$(credentials: Credentials): Observable<void> {
        return from(encryptRSA(credentials.password)).pipe(
            tap((encryptedPassword) => {
                credentials.password = encryptedPassword;
            }),
            switchMap(() => this.api.post$<AuthResponse>('auth/login', credentials)),
            switchMap((response) => {
                this.bearer = {
                    accessToken: response.accessToken,
                    expiresAt: response.expiresAt,
                };
                this.store.dispatch(new SetUser(response.user));
                return this.store.dispatch(new SetBearer(this.bearer));
            }),
            map(() => this.isAuthenticatedSubject.next(true)),
            switchMap(() => this.navigateToDefaultUserPage$()),
        );
    }

    /**
     * @description
     * Déconnecte l'utilisateur.
     */
    public logout$(): Observable<void> {
        return forkJoin([
            of(this.isAuthenticatedSubject.next(false)),
            of(this.bearer = null),
        ]).pipe(
            switchMap(() => this.store.dispatch([new ResetUser()])),
            switchMap(() => this.navigateToDefaultGuestPage$()),
        );
    }

    /**
     * @description
     * Vérifie si l'utilisateur est authentifié.
     * Si authentifié, redirige vers la page d'accueil, sinon la page de connexion.
     */
    public checkAuthenticationState$(): Observable<void> {
        this.bearer = this.store.selectSnapshot(UserState.getBearer);

        return this.api.post$('auth/verify').pipe(
            switchMap(() => {
                this.isAuthenticatedSubject.next(true);

                if(this.router.url.startsWith('/auth'))
                    return this.navigateToDefaultUserPage$();

                return of(void 0);
            }),
            catchError(() => of(void 0))
        );
    }
}
