/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { ChangeDetectionStrategy, Component, OnInit, viewChild } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { tap } from 'rxjs';
import { AuthService } from 'src/app/core/services/Auth.service';
import { NavigationLoaderService } from 'src/app/core/services/NavigationLoader.service';
import { SubscriptionManager } from 'src/app/shared/directives/SubscriptionManager.directive';
import { LoadingComponent } from 'src/app/shared/ui/components/loading/loading.component';
import { FooterComponent } from "./shared/components/footer/footer.component";

@Component({
    selector: 'app-root',
    templateUrl: 'app.component.html',
    styleUrls: ['app.component.scss'],
    standalone: true,
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [RouterOutlet, LoadingComponent, FooterComponent],
})
export class AppComponent extends SubscriptionManager implements OnInit {
    protected readonly loadingScreen = viewChild.required<LoadingComponent>('loadingScreen');

    constructor(
        private readonly auth: AuthService,
        private readonly navigationLoader: NavigationLoaderService,
    ) {
        super();
    }

    public ngOnInit(): void {
        this.watch$ = this.auth.checkAuthenticationState$().pipe(
            tap(() => setTimeout(() => {
                this.loadingScreen().dismiss();
            }, 500)),
        );
    }
}
