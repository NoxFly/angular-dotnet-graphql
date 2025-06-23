/* eslint-disable @typescript-eslint/no-explicit-any */
/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { NgIf } from '@angular/common';
import { ChangeDetectionStrategy, Component, ElementRef, forwardRef, HostBinding, input, signal, viewChild } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { randomId } from 'src/app/shared/helpers/utils';
import { IconComponent } from "../icon/icon.component";

// https://developer.mozilla.org/fr/docs/Web/HTML/Reference/Attributes/autocomplete
type Autocomplete =
    | 'off'
    | 'on'
    // -- mot de passe
    | 'new-password'
    | 'current-password'
    | 'one-time-code'
    // -- nom
    | 'honorific-prefix'
    | 'given-name'
    | 'additional-name'
    | 'family-name'
    | 'honorific-suffix'
    | 'nickname'
    // --
    | 'email'
    | 'username'
    // --
    | 'organization-title'
    | 'organization'
    // --
    | 'street-address'
    | 'address-line1'
    | 'address-line2'
    | 'address-line3'
    | 'address-level4'
    | 'address-level3'
    | 'address-level2'
    | 'address-level1'
    | 'country'
    | 'country-name'
    | 'postal-code'
    // -- carte bancaire
    | 'cc-name'
    | 'cc-given-name'
    | 'cc-additional-name'
    | 'cc-family-name'
    | 'cc-number'
    | 'cc-exp'
    | 'cc-exp-month'
    | 'cc-exp-year'
    | 'cc-csc'
    | 'cc-type'
    // -- transaction
    | 'transaction-currency'
    | 'transaction-amount'
    // --
    | 'language'
    | 'bday'
    | 'bday-day'
    | 'bday-month'
    | 'bday-year'
    | 'sex'
    | 'tel'
    | 'tel-extension'
    | 'impp'
    | 'url'
    | 'photo'
    ;

type InputType =
    | 'text'
    | 'password'
    | 'email'
    | 'number'
    | 'checkbox'
    | 'radio'
    | 'file'
    | 'search'
    | 'tel'
    | 'url'
    ;
    // ce composant ne gère pas les types suivants :
    // hidden | date | time | datetime-local | month | week | color | range | image
    // si implémenté, d'autres composants spécifiques à ces types devront être utilisés.

@Component({
    selector: 'ui-input',
    standalone: true,
    templateUrl: './input.component.html',
    styleUrls: ['./input.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => InputComponent),
            multi: true
        }
    ],
    imports: [NgIf, IconComponent]
})
export class InputComponent implements ControlValueAccessor {
    public type = input.required<InputType>();
    public placeholder = input<string>('');
    public disabled = input<boolean>(false);
    public togglePassword = input<boolean>(false);
    public pattern = input<string>('');
    public label = input<string | undefined>(undefined);
    public autocomplete = input<Autocomplete>('off');

    public value = signal<string>('');

    protected _disabled = signal<boolean>(false);

    protected id = randomId();

    public readonly inputElement = viewChild.required<ElementRef<HTMLInputElement>>('input');

    @HostBinding('class.has-focus')
    public hasFocus: boolean = false;

    @HostBinding('class.show-password')
    public get showPassword(): boolean {
        return this.type() === 'password' && this.inputElement().nativeElement.type === 'text';
    }

    // ---

    public onChange = (value: any): void => {};
    public onTouched = (): void => {};

    public registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    public registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    // ---

    public writeValue(value: any): void {
        this.value.set(value);
    }

    public valueChanged(value: string): void {
        this.onChange(value);
        this.value.set(value);
    }

    public handleInput(event: Event): void {
        const input = event.target as HTMLInputElement;
        this.value.set(input.value);
        this.onChange(this.value());
    }

    public handleBlur(): void {
        this.hasFocus = false;
        this.onTouched();
    }

    public handleFocus(): void {
        this.hasFocus = true;
        this.onTouched();
    }


    // ---

    public setDisabledState(isDisabled: boolean): void {
        this._disabled.set(isDisabled);
    }

    public setFocus(): void {
        this.inputElement().nativeElement.focus();
    }

    // ---

    protected togglePasswordVisibility(): void {
        const input = this.inputElement().nativeElement;

        input.type = input.type === 'text'
            ? 'password'
            : 'text';
    }
}
