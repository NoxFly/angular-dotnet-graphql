/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { Resource } from 'src/app/core/models/dto/resource';
import { Maybe, StateBuffer } from 'src/app/core/models/misc';
import { UserStateModel } from 'src/app/core/models/states/userState';
import { ResetUser, SetBearer, SetUser } from './User.action';
import { Bearer } from 'src/app/core/models/dto/auth';

@State<UserStateModel>({
    name: 'user',
    defaults: new UserStateModel(),
})
@Injectable()
export class UserState {
    @Selector()
    public static getUser(state: UserStateModel): StateBuffer<Maybe<Resource>> {
        return {
            data: state.user,
            lastUpdate: state.userLastUpdate
        };
    }

    @Selector()
    public static getBearer(state: UserStateModel): Maybe<Bearer> {
        return state.bearer;
    }

    @Action(SetUser)
    public setUser(ctx: StateContext<UserStateModel>, action: SetUser): void {
        ctx.patchState({
            user: action.user,
            userLastUpdate: Date.now(),
        });
    }

    @Action(SetBearer)
    public setBearer(ctx: StateContext<UserStateModel>, action: SetBearer): void {
        ctx.patchState({
            bearer: action.bearer,
        });
    }

    @Action(ResetUser)
    public resetUser(ctx: StateContext<UserStateModel>): void {
        ctx.setState(new UserStateModel());
    }
}
