/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { Injectable } from '@angular/core';
import { NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';
import { Maybe } from 'src/app/core/models/misc';
import { LoadingComponent } from 'src/app/shared/ui/components/loading/loading.component';
import { LoadingController } from 'src/app/shared/ui/components/loading/loading.controller';

@Injectable({
    providedIn: 'root'
})
export class NavigationLoaderService {
    private readonly navigationTimeoutDuration = 500;
    private loader: LoadingComponent | null = null;
    private navigationTimeout: Maybe<number> = null;

    constructor(
        private readonly router: Router,
        private readonly loadingCtrl: LoadingController
    ) {
        this.router.events.subscribe(event => {
            if(event instanceof NavigationStart) {
                this.navigationTimeout = setTimeout(() => {
                    this.showLoader();
                }, this.navigationTimeoutDuration); // Show loader if navigation takes more than 500ms
            }
            else if(event instanceof NavigationEnd || event instanceof NavigationCancel || event instanceof NavigationError) {
                if(this.navigationTimeout) {
                    clearTimeout(this.navigationTimeout);
                    this.navigationTimeout = null;
                    this.hideLoader();
                }
            }
        });
    }

    public async showLoader(): Promise<void> {
        this.loader = await this.loadingCtrl.create();
    }

    public async hideLoader(): Promise<void> {
        if(this.loader) {
            await this.loader.dismiss();
            this.loader = null;
        }
    }
}
