/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { NgFor, NgIf } from '@angular/common';
import { Component, HostBinding, input, computed, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { UIComponent } from 'src/app/shared/ui/UIComponent.directive';
import { ToastPosition, UIAction, UIColor } from 'src/app/shared/ui/ui.types';

@Component({
    selector: 'ui-toast',
    standalone: true,
    templateUrl: './toast.component.html',
    styleUrls: ['./toast.component.scss'],
    imports: [NgFor, NgIf],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ToastComponent extends UIComponent implements OnInit {
    public message  = input.required<string>();
    public duration = input<number>();
    public color    = input<UIColor>();
    public closable = input<boolean>();
    public position = input<ToastPosition>();
    public actions  = input<UIAction[]>([]);


    @HostBinding('attr.data-toast-type')
    public get colorClass(): string {
        return this.color() || 'medium';
    }

    @HostBinding('attr.data-toast-position')
    public get positionClass(): string {
        return this.position() || 'top-center';
    }


    protected hasActions = computed(() => this.actions().length > 0);
    protected hasDuration = computed(() => this.duration() !== undefined);

    public ngOnInit(): void {
        if(this.hasDuration()) {
            this.ref.nativeElement.style.setProperty('--toast-duration', `${this.duration()}ms`);
            this.ref.nativeElement.classList.add('ephemeral');

            this.ref.nativeElement.addEventListener('animationend', () => {
                this.dismiss();
            }, { once: true });
        }
    }
}
