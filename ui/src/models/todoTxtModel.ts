import {DueStatusType} from "@/services/todoTxtService";

export interface TodoTxtModel {
    fullText: string;
    priority: string;
    completed: boolean;
    completionDate?: Date;
    creationDate?: Date;
    dueDate?: Date;
    dueStatus: DueStatusType;
    description: string;
    projectTags: string[];
    contextTags: string[];
}