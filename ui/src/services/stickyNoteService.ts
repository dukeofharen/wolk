import {StickyNotesModel} from "@/models/stickyNotesModel";

function parseTitle(line: string): string {
    return line.replace(/=====/g, "").trim();
}

function parseMetadata(line: string): string {
    return JSON.parse(line.replace(/-----/g, "").trim());
}

export function stickyNotesToModel(input: string): StickyNotesModel[] {
    let result: StickyNotesModel[] = [];

    let lines = input.lines();
    for (let line of lines) {
        if (line.indexOf("=====") > -1) {
            // New sticky note...
            result.push({
                title: parseTitle(line),
                contents: "",
                metadata: {
                    backgroundColor: "#FFFFFF"
                }
            });
        } else if (line.indexOf("-----") > -1) {
            result[result.length - 1].metadata = parseMetadata(line);
        } else if (result.length > 0) {
            result[result.length - 1].contents += `${line}\n`;
        }
    }

    // Trim new lines of results...
    for (let note of result) {
        note.contents = note.contents.trim();
    }

    return result;
}

export function stickyNotesToString(notes: StickyNotesModel[]) {
    let result = "";
    for (let note of notes) {
        result += `===== ${note.title} =====\n`;
        result += `----- ${JSON.stringify(note.metadata)} -----\n\n`;
        result += `${note.contents}\n\n`;
    }

    return result;
}