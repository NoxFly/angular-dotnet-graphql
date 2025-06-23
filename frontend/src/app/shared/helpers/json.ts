/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

export async function parseJSON<T=unknown>(json: string): Promise<T> {
    return new Promise((resolve, reject) => {
        try {
            resolve(JSON.parse(json));
        }
        catch(error) {
            reject(error);
        }
    });
}

export function parseJSONSync<T=unknown>(json: string): T {
    try {
        return JSON.parse(json);
    }
    catch(error) {
        throw error;
    }
}

export function deepCopy<T>(object: T): T {
    return JSON.parse(JSON.stringify(object));
}


export function partition<T>(data: T[], comparator: (el: T) => boolean): T[][] {
    const res: T[][] = [[], []];

    for(const item of data) {
        if(comparator(item)) {
            res[0].push(item);
        }
        else {
            res[1].push(item);
        }
    }

    return res;
}
