// Source: https://stackoverflow.com/questions/400212/how-do-i-copy-to-the-clipboard-in-javascript
function fallbackClipboardCopy(text: string) {
    let textArea = document.createElement("textarea");
    textArea.value = text;
    textArea.style.position = "fixed";  //avoid scrolling to bottom
    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();

    try {
        let successful = document.execCommand("copy");
        if (!successful) {
            throw "Could not copy text to clipboard.";
        }
    } finally {
        document.body.removeChild(textArea);
    }
}

export function clipboardCopy(text: string) {
    if (!navigator.clipboard) {
        fallbackClipboardCopy(text);
        return;
    }

    navigator.clipboard.writeText(text).then(() => {
    }, err => {
        throw err;
    });
}