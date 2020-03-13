import {NoteType} from "@/models/api/enums/noteType";

export interface LoadNotesQueryModel {
    notebookId?: number;
    searchTerm?: string;
    noteType?: NoteType;
    includeFullContent?: boolean;
};