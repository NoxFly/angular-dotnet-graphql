/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

export async function importPublicKey(spkiPem: string): Promise<CryptoKey> {
    return await window.crypto.subtle.importKey(
        "spki",
        getSpkiDer(spkiPem),
        {
            name: "RSA-OAEP",
            hash: "SHA-256",
        },
        true,
        ["encrypt"]
    );
}

export async function encryptRSA(key: CryptoKey, plaintext: string): Promise<string> {
    const encrypted = await window.crypto.subtle.encrypt(
        {
            name: "RSA-OAEP"
        },
        key,
        new TextEncoder().encode(plaintext)
    );

    return window.btoa(ab2str(encrypted));
}

// ---

function getSpkiDer(spkiPem: string): ArrayBuffer {
    const pemHeader = "-----BEGIN PUBLIC KEY-----";
    const pemFooter = "-----END PUBLIC KEY-----";
    const pemContents = spkiPem
        .substring(pemHeader.length, spkiPem.length - pemFooter.length)
        .replace(/\n\s/g, '');
    const binToStr = window.atob(pemContents);
    return str2ab(binToStr);
}

function str2ab(str: string): ArrayBuffer {
    const buf = new ArrayBuffer(str.length);
    const bufView = new Uint8Array(buf);

    for(let i = 0, strLen = str.length; i < strLen; i++) {
        bufView[i] = str.charCodeAt(i);
    }

    return buf;
}

function ab2str(buf: ArrayBuffer): string {
    return String.fromCharCode.apply(null, new Uint8Array(buf) as unknown as number[]);
}
