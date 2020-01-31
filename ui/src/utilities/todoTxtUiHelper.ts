import {TodoTxtModel} from "@/models/todoTxtModel";
import {TodoTxtFilterModel} from "@/models/store/todoTxtFilterModel";
import marked from "marked";
import {DueStatusType} from "@/models/enums/dueStatusType";
import {timeUnitsInSeconds} from "@/resources";

export function filterTodoItems(models: TodoTxtModel[], filter: TodoTxtFilterModel) {
    let result = models;
    if (filter.projectTagFilter) {
        result = result.filter(r => r.projectTags.indexOf(filter.projectTagFilter) > -1);
    }

    if (filter.contextTagFilter) {
        result = result.filter(r => r.contextTags.indexOf(filter.contextTagFilter) > -1);
    }

    if (filter.excludeDone) {
        result = result.filter(r => !r.completed);
    }

    let dueSeconds: number = -1;
    if (filter.dueStatus === DueStatusType.OVERDUE) {
        result = result.filter(r => r.dueStatus === DueStatusType.OVERDUE);
    } else if (filter.dueStatus === DueStatusType.DUE_IN_A_MONTH) {
        dueSeconds = timeUnitsInSeconds.month;
    } else if (filter.dueStatus === DueStatusType.DUE_IN_A_WEEK) {
        dueSeconds = timeUnitsInSeconds.week;
    } else if (filter.dueStatus === DueStatusType.DUE_IN_A_DAY) {
        dueSeconds = timeUnitsInSeconds.day * 2;
    } else if (filter.dueStatus === DueStatusType.DUE_TODAY) {
        dueSeconds = timeUnitsInSeconds.day;
    }

    if (dueSeconds > -1) {
        result = result.filter(r => r.secondsToDue && r.secondsToDue <= dueSeconds);
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

export function getPriorities(): string[] {
    return [
        '',
        'A',
        'B',
        'C',
        'D',
        'E',
        'F'
    ];
}