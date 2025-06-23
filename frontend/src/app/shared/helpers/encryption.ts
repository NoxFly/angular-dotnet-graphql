/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

import * as RSA from 'src/app/shared/helpers/rsa';
import { environment } from 'src/environments/environment';

export async function encryptRSA(text: string): Promise<string> {
    const publicKey = '-----BEGIN PUBLIC KEY-----'
        + environment.rsaPublicKey
        + '-----END PUBLIC KEY-----';

    const pub = await RSA.importPublicKey(publicKey);
    return await RSA.encryptRSA(pub, text);
}
