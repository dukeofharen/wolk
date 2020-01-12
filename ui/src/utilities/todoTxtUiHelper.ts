import {TodoTxtModel} from "@/models/todoTxtModel";
import {TodoTxtFilterModel} from "@/models/store/todoTxtFilterModel";

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