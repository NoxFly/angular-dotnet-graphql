/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, viewChild } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { catchError, finalize } from 'rxjs';
import { Credentials } from 'src/app/core/models/dto/auth';
import { AuthService } from 'src/app/core/services/Auth.service';
import { SubscriptionManager } from 'src/app/shared/directives/SubscriptionManager.directive';
import { InputComponent } from 'src/app/shared/ui/components/input/input.component';
import { LoadingController } from 'src/app/shared/ui/components/loading/loading.controller';
import { ToastController } from 'src/app/shared/ui/components/toast/toast.controller';

@Component({
    selector: 'app-login',
    templateUrl: 'login.page.html',
    styleUrls: ['login.page.scss'],
    standalone: true,
    imports: [ReactiveFormsModule, InputComponent],
})
export class LoginPage extends SubscriptionManager implements OnInit {
    protected form!: FormGroup;

    protected readonly passwordInput = viewChild.required<InputComponent>('passwordInput');

    constructor(
        private readonly auth: AuthService,
        private readonly loadingCtrl: LoadingController,
        private readonly toastCtrl: ToastController,
    ) {
        super();
    }

    protected focusPassword(): void {
        this.passwordInput()?.setFocus();
    }

    private async onAttemptFailed(error: HttpErrorResponse): Promise<void> {
        console.error('login failed', error);

        let message: string;

        switch(error.status) {
            case 400:
            case 401:
            case 403:
                message = 'Identifiants incorrects'; break;
            case 405:
            case 406:
            case 500:
                message = 'Erreur interne (code=1)'; break;
            case 404:
            case 501:
            case 502:
            case 503:
            case 504:
                message = 'Service indisponible'; break;
            default:
                message = 'Erreur interne (code=2)'; break;
        }

        await this.toastCtrl.create({
            color: 'error',
            duration: 5000,
            message,
            position: 'top-center',
        });
    }

    protected async attemptLogin(): Promise<void> {
        if(this.form.invalid) {
            return;
        }

        const creds: Credentials = {
            id: this.form.get('id')?.value,
            password: this.form.get('password')?.value,
        };

        const loader = await this.loadingCtrl.create({
            message: 'Connexion en cours...',
        });

        this.watch$ = this.auth.login$(creds).pipe(
            catchError((error: HttpErrorResponse) => {
                this.onAttemptFailed(error);
                return [];
            }),
            finalize(() => loader.dismiss()),
        );
    }

    public ionViewWillEnter(): void {
        this.form.reset();
    }

    public ngOnInit(): void {
        this.form = new FormGroup({
            // honeypot contre les bots
            email: new FormControl('', [
                Validators.maxLength(0),
            ]),
            id: new FormControl('', [
                Validators.required,
                Validators.minLength(3),
                Validators.maxLength(128)
            ]),
            password: new FormControl('', [
                Validators.required,
                Validators.maxLength(256)
            ]),
        });
    }
}
