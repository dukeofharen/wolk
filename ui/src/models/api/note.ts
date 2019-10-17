import { NoteType } from './enums/noteType';

export default interface Note {
    id: number;
    title: string;
    content: string;
    preview: string;
    noteType: NoteType;
    notebookId: number;
    created: Date;
    updated: Date;
}