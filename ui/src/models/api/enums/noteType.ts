import { KeyValuePair } from '@/models/keyValuePair';

export enum NoteType {
    NotSet = 0,
    PlainText = 1,
    Markdown = 2,
    TodoTxt = 3
};

export const NoteTypeMap = new Map<NoteType, string>([
    [NoteType.Markdown, 'Markdown'],
    [NoteType.PlainText, 'Plain text'],
    [NoteType.TodoTxt, 'todo.txt']
]);

export function getNoteTypeArray(): Array<KeyValuePair<NoteType, string>> {
    let result: Array<KeyValuePair<NoteType, string>> = [];
    let keys = Array.from(NoteTypeMap.keys());
    for (let key of keys) {
        result.push({
            key: key,
            value: NoteTypeMap.get(key) || ''
        });
    }

    return result;
}