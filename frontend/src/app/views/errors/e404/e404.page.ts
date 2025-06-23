/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FooterComponent } from "src/app/shared/components/footer/footer.component";
import { NavigationComponent } from "src/app/shared/components/navigation/navigation.component";

@Component({
    selector: 'app-e404',
    templateUrl: './e404.page.html',
    styleUrls: ['./e404.page.scss'],
    standalone: true,
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [RouterLink, NavigationComponent, FooterComponent],
})
export class E404Page {}
