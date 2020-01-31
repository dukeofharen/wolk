import {DueStatusType} from "@/models/enums/dueStatusType";

export interface TodoTxtModel {
    noteId?: number;
    fullText: string;
    priority: string;
    completed: boolean;
    completionDate?: string;
    creationDate?: string;
    dueDate?: string;
    dueStatus?: DueStatusType;
    secondsToDue?: number;
    description: string;
    projectTags: string[];
    contextTags: string[];
    hashCode: number;
}