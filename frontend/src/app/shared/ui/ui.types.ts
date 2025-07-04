/* eslint-disable @typescript-eslint/no-explicit-any */
/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { Type } from "@angular/core";

/* UI - Global */

export type UIButtonRole =
    | 'cancel'
    | 'confirm'
    | 'none'
    | 'destructive';

export type UIColor =
    | 'primary'
    | 'secondary'
    | 'error'
    | 'success'
    | 'warning'
    | 'light'
    | 'dark'
    | 'medium'
    | 'default';

export type UIAction = {
    text: string;
    role: UIButtonRole;
    handler?: (self: UIAction) => void;
};

export type UIIconAction = UIAction & {
    icon?: string;
};

export type UIDismissData = {
    role: UIButtonRole;
    data: any;
};

export type UIConfig = {
    id?: string;
    classes?: string;
};


/* Alert */

export type AlertConfig = UIConfig & {
    title?: string;
    message?: string;
    duration?: number;
    details?: string;
    actions?: UIAction[];
};


/* Modal */

export type ModalConfig = UIConfig & {
    component: Type<any>;
    componentProps?: Record<string, any>;
    showBackdrop?: boolean;
    showDots?: boolean;
    backdropClose?: boolean;
    keyboardClose?: boolean;
};


/* Toast */

export type ToastPosition =
    | 'top-left'
    | 'top-center'
    | 'top-right'
    | 'bottom-left'
    | 'bottom-center'
    | 'bottom-right';

export type ToastConfig = UIConfig & {
    message: string;
    duration?: number;
    actions?: UIIconAction[];
    color?: UIColor;
    closable?: boolean;
    position?: ToastPosition;
};

export type LoadingConfig = UIConfig & {
    message?: string;
};


/* Select */

export type SelectOption<T> = UIIconAction & {
    value: T;
};
