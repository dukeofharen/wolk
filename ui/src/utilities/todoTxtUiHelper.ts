import {TodoTxtModel} from "@/models/todoTxtModel";
import {TodoTxtFilterModel} from "@/models/store/todoTxtFilterModel";
import {DueStatusType} from "@/services/todoTxtService";
import marked from "marked";

export function filterTodoItems(models: TodoTxtModel[], filter: TodoTxtFilterModel) {
    let result = models;
    if (filter.projectTagFilter) {
        result = result.filter(r => r.projectTags.indexOf(filter.projectTagFilter) > -1);
    }

    if (filter.contextTagFilter) {
        result = result.filter(r => r.contextTags.indexOf(filter.contextTagFilter) > -1);
    }

    return result;
}

export function getDueStatusColor(model: TodoTxtModel) {
    let defaultColor = "#ffffff";
    if (model.completed) {
        return defaultColor;
    }

    switch (model.dueStatus) {
        case DueStatusType.OVERDUE:
            return "#ff8f8f";
        case DueStatusType.DUE_TODAY:
            return "#ffcf8f";
        case DueStatusType.DUE_IN_A_DAY:
            return "#faff8f";
        default:
            return defaultColor;
    }
}

export function parseMarkdown(input: string) {
    return marked.inlineLexer(input, []);
}