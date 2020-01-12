import {DueStatusType} from "@/models/enums/dueStatusType";

export interface TodoTxtFilterModel {
    projectTagFilter: string;
    contextTagFilter: string;
    excludeDone: boolean;
    dueStatus: DueStatusType;
}