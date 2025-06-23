/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { Bearer } from "src/app/core/models/dto/auth";
import { Resource } from "src/app/core/models/dto/resource";
import { Maybe } from "src/app/core/models/misc";

export class SetUser {
    public static readonly type = '[User] Set';

    constructor(public user: Resource) {}
}

export class ResetUser {
    public static readonly type = '[User] Reset';
}

export class SetBearer {
    public static readonly type = '[User] Set Bearer';

    constructor(public bearer: Maybe<Bearer>) {}
}
