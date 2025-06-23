/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { Resource } from "src/app/core/models/dto/resource";

/**
 * @description
 * Les informations d'identification d'un utilisateur.
 */
export type Credentials = {
    id: string;
    password: string;
};

export type Bearer = {
    accessToken: string;
    expiresAt: number;
};

/**
 * @description
 * La réponse d'authentification contenant les informations de l'utilisateur
 * et le token CSRF qui sera à inclure dans les requêtes ultérieures.
 */
export type AuthResponse = {
    user: Resource;
} & Bearer;
