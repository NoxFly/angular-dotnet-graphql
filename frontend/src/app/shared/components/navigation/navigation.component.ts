/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { Location, NgIf } from '@angular/common';
import { Component, input, signal } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthService } from 'src/app/core/services/Auth.service';
import { SubscriptionManager } from 'src/app/shared/directives/SubscriptionManager.directive';
import { LoadingController } from 'src/app/shared/ui/components/loading/loading.controller';
import { IconComponent } from "src/app/shared/ui/components/icon/icon.component";

@Component({
    selector: 'app-navigation',
    templateUrl: './navigation.component.html',
    styleUrls: ['./navigation.component.scss'],
    standalone: true,
    imports: [NgIf, IconComponent],
})
export class NavigationComponent extends SubscriptionManager {
    public readonly backButton = input<boolean>(false);
    public readonly backHref = input<string>();

    protected readonly isProfileMenuOpen = signal<boolean>(false);

    constructor(
        private readonly auth: AuthService,
        private readonly router: Router,
        private readonly location: Location,
        private readonly loadingCtrl: LoadingController,
    ) {
        super();
    }

    protected goBack(): void {
        const backHref = this.backHref();

        if(backHref) {
            this.router.navigateByUrl(backHref);
        }
        else {
            this.location.back();
        }
    }

    protected toggleProfileMenu(): void {
        this.isProfileMenuOpen.update(v => !v);
    }

    protected goToHomePage(): void {
        this.router.navigateByUrl('/dashboard/home', { replaceUrl: true });
    }

    protected async logout(): Promise<void> {
        const loader = await this.loadingCtrl.create();

        this.watch$ = this.auth.logout$().pipe(
            finalize(() => loader.dismiss()),
        );
    }
}
