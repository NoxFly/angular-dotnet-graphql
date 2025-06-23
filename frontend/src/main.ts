/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { registerLocaleData } from '@angular/common';
import localeExtraFr from "@angular/common/locales/extra/fr";
import localeFr from "@angular/common/locales/fr";
import { enableProdMode } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { AppComponent } from './app/app.component';
import { appConfig } from './app/core/appConfig';


if(environment.production) {
    enableProdMode();
}

registerLocaleData(localeFr, "fr-FR", localeExtraFr);

bootstrapApplication(AppComponent, appConfig)
    .catch(console.error);
