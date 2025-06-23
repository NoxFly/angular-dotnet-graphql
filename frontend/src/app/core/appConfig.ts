/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { DecimalPipe } from '@angular/common';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { ApplicationConfig, DEFAULT_CURRENCY_CODE, LOCALE_ID, provideExperimentalZonelessChangeDetection } from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';
import { withNgxsLoggerPlugin } from "@ngxs/logger-plugin";
import { withNgxsStoragePlugin } from "@ngxs/storage-plugin";
import { provideStore } from '@ngxs/store';
import { routes } from 'src/app/app.routes';
import { RequestInterceptor } from 'src/app/core/interceptors/be.interceptor';

import { AuthService } from 'src/app/core/services/Auth.service';
import { NavigationLoaderService } from 'src/app/core/services/NavigationLoader.service';
import { UserState } from 'src/app/core/states/User/User.state';
import { environment } from 'src/environments/environment';

export const appConfig: ApplicationConfig = {
    providers: [
        // Locale
        { provide: LOCALE_ID, useValue: "fr-FR" },
        { provide: DEFAULT_CURRENCY_CODE, useValue: "EUR" },
        // Interceptors
        { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptor, multi: true },
        // Ionic
        // Router
        provideRouter(routes, withViewTransitions()),
        provideHttpClient(withInterceptorsFromDi()),
        provideExperimentalZonelessChangeDetection(),

        // NGXS
        provideStore(
            // les différents stores de l'application
            [UserState],
            {
                developmentMode: !environment.production,
            },
            // ceux qui doivent être persistents
            // si `keys: '*'` alors sauvegarde tous les states mentionnés au dessus
            // avec comme clé '@@STATE' et comme valeur { foo: {...}, bar: {...} }.
            // sinon keys vaut une liste de noms de states, par exemple ['foo', 'bar']
            withNgxsStoragePlugin({
                keys: '*',
            }),
            withNgxsLoggerPlugin({
                logger: console,
                collapsed: true,
                disabled: !!environment.production
            }),
        ),

        // Si des choses ne peuvent pas être injectées directement
        // dans l'instance cible (ex: Pipe), l'injecter au top-level ici.
        { provide: DecimalPipe, useClass: DecimalPipe },

        // Singleton services
        NavigationLoaderService,
        AuthService,
    ],
};
