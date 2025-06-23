/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { Bearer } from "src/app/core/models/dto/auth";
import { Resource } from "src/app/core/models/dto/resource";
import { Maybe } from "src/app/core/models/misc";

export class UserStateModel {
    public user: Maybe<Resource> = null;
    public userLastUpdate: number = 0;

    public bearer: Maybe<Bearer> = null;
}
