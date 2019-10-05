export function getSessionValue(key: string): any {
    let item = sessionStorage.getItem(key);
    if (!item) {
        return null;
    }

    return JSON.parse(item);
}

export function setSessionValue(key: string, value: any) {
    sessionStorage.setItem(key, JSON.stringify(value));
}

export function getLocalValue(key: string): any {
    let item = localStorage.getItem(key);
    if (!item) {
        return null;
    }

    return JSON.parse(item);
}

export function setLocalValue(key: string, value: any) {
    localStorage.setItem(key, JSON.stringify(value));
}