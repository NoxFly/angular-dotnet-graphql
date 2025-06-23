/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { HttpParams } from "@angular/common/http";

export type Maybe<T> = T | null;

export type StateBuffer<T> = {
    data: T;
    lastUpdate: number;
};

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export type Query = HttpParams | Record<string, any>;

export type Mapper<T> = (data: T) => T;

export type DateOnly = string; // ISO dateonly : "YYYY-MM-DD"
