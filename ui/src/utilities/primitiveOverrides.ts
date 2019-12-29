// Source: https://stackoverflow.com/questions/20070158/string-format-not-work-in-typescript
interface String {
    format(...replacements: string[]): string;

    hashCode(): number;
    
    lines(): string[];
}

if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
                ? args[number]
                : match
                ;
        });
    };
}

// Source: https://stackoverflow.com/questions/6122571/simple-non-secure-hash-function-for-javascript
if (!String.prototype.hashCode) {
    String.prototype.hashCode = function () {
        let hash = 0;
        if (this.length == 0) {
            return hash;
        }
        
        for (let i = 0; i < this.length; i++) {
            let char = this.charCodeAt(i);
            hash = ((hash << 5) - hash) + char;
            hash = hash & hash; // Convert to 32bit integer
        }
        
        return hash;
    }
}

if(!String.prototype.lines) {
    String.prototype.lines = function() {
        return this.split(/\r?\n/);
    }
}