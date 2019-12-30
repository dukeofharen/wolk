export interface TodoTxtModel {
    fullText: string;
    priority: string;
    completed: boolean;
    completionDate?: Date;
    creationDate?: Date;
    description: string;
    projectTags: string[];
    contextTags: string[];
}