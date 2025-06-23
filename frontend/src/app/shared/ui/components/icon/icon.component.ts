/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { NgIf } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, input, OnInit, signal, ViewEncapsulation } from '@angular/core';
import { SanitizePipe } from 'src/app/shared/pipes/sanitize.pipe';

@Component({
    selector: 'ui-icon',
    standalone: true,
    templateUrl: './icon.component.html',
    styleUrls: ['./icon.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [NgIf, SanitizePipe]
})
export class IconComponent implements OnInit {
    public src = input.required<string>();
    public color = input<string>('');

    protected content = signal<string>('');

    protected isSVG = computed((): boolean => {
        return this.src().endsWith('.svg');
    });

    protected isImage = computed((): boolean => {
        const img = this.src();
        return img.endsWith('.png')
            || img.endsWith('.jpg')
            || img.endsWith('.jpeg');
    });

    private async fetchInlineSVG(): Promise<void> {
        try {
            const response = await fetch(this.src());

            if(!response.ok) {
                throw new Error(`HTTP error status: ${response.status}`);
            }

            const svgStr = await response.text();

            const parser = new DOMParser();
            const svgDoc = parser.parseFromString(svgStr, 'image/svg+xml');

            const svg = svgDoc.querySelector('svg');

            if(svg) {
                svg.removeAttribute('width');
                svg.removeAttribute('height');
            }

            this.content.set(svgDoc.documentElement.outerHTML);
        }
        catch(error) {
            console.error('File not found:', this.src(), error);
        }
    }

    public ngOnInit(): void {
        if(this.isSVG()) {
            this.fetchInlineSVG();
        }
    }

}
