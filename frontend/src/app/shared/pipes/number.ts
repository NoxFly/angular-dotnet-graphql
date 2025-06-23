/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { DecimalPipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'number',
    standalone: true,
})
export class NumberPipe implements PipeTransform {
    private readonly zeroVal = '-';

    constructor(private readonly decimal: DecimalPipe) {}

    public transform(value: number): string {
        if(value === 0)
            return this.zeroVal;

        if(value % 1 === 0)
            return value.toString();

        return this.decimal.transform(value, '1.1-2') ?? this.zeroVal;
    }

}
