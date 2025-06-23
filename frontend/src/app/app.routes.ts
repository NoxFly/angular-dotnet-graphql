/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { Routes } from '@angular/router';
import { AuthGuard } from 'src/app/core/guards/Auth.guard';
import { GuestGuard } from 'src/app/core/guards/Guest.guard';

export const routes: Routes = [
    {
        path: "",
        redirectTo: "dashboard",
        pathMatch: "full",
    },
    // ---- Dashboard (member section) ----
    {
        path: "dashboard",
        canActivate: [AuthGuard],
        loadChildren: () => import("./views/dashboard/dashboard.routes").then((d) => d.routes),
    },
    // ---- Guest (public section) ----
    {
        path: "auth/login",
        canActivate: [GuestGuard],
        loadComponent: () => import("./views/guest/login/login.page").then(m => m.LoginPage),
    },
    // 404
    {
        path: "**",
        pathMatch: "full",
        loadComponent: () => import("./views/errors/e404/e404.page").then(m => m.E404Page),
    }
];
