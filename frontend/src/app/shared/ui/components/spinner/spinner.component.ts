/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { ChangeDetectionStrategy, Component, input, ViewEncapsulation } from '@angular/core';

@Component({
    selector: 'ui-spinner',
    standalone: true,
    templateUrl: './spinner.component.html',
    styleUrl: './spinner.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    encapsulation: ViewEncapsulation.None,
})
export class SpinnerComponent {
    public color = input<string>('');
}
