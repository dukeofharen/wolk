import {DueStatusType} from "@/models/enums/dueStatusType";

export interface TodoTxtModel {
    noteId?: number;
    fullText: string;
    priority: string;
    completed: boolean;
    completionDate?: Date;
    creationDate?: Date;
    dueDate?: Date;
    dueStatus?: DueStatusType;
    secondsToDue?: number;
    description: string;
    projectTags: string[];
    contextTags: string[];
    hashCode: number;
}