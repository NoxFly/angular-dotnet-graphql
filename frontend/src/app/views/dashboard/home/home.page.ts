/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { ChangeDetectionStrategy, Component } from '@angular/core';
import { NavigationComponent } from 'src/app/shared/components/navigation/navigation.component';

@Component({
    selector: 'app-home',
    templateUrl: 'home.page.html',
    styleUrls: ['home.page.scss'],
    standalone: true,
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [NavigationComponent],
})
export class HomePage {
}
