import {TodoTxtModel} from "@/models/todoTxtModel";
import {firstBy} from "thenby";
import moment from 'moment'

export function singleTodoTxtToModel(line: string): TodoTxtModel {
    let model: TodoTxtModel = {
        completed: false,
        priority: "",
        completionDate: undefined,
        creationDate: undefined,
        contextTags: [],
        description: "",
        fullText: line,
        projectTags: []
    };

    let parts = line.trim().split(" ");

    // Check completed...
    if (parts[0] === "x") {
        model.completed = true;
        parts.shift();
    }

    // Check priority...
    if (parts[0].indexOf("(") === 0) {
        model.priority = parts[0].substring(1, 2);
        parts.shift();
    }

    // Check dates...
    let dateReg = /^\d{4}[./-]\d{2}[./-]\d{2}$/;
    if (parts[0].match(dateReg) && parts[1].match(dateReg)) {
        // Both created and completed are set.
        model.completionDate = new Date(parts[0]);
        model.creationDate = new Date(parts[1]);
        parts.shift();
        parts.shift();
    } else if (parts[0].match(dateReg)) {
        // Only created is set.
        model.creationDate = new Date(parts[0]);
        parts.shift();
    }

    model.description = parts.join(" ");
    for (let part of parts) {
        if (part.indexOf("+") === 0) {
            model.projectTags.push(part);
        }

        if (part.indexOf("@") === 0) {
            model.contextTags.push(part);
        }
    }

    return model;
}

export function todoTxtToModels(input: string): TodoTxtModel[] {
    let result: TodoTxtModel[] = [];
    let lines = input.lines();
    for (let line of lines) {
        if (!line) {
            continue;
        }

        result.push(singleTodoTxtToModel(line));
    }

    // Retrieve all not-done models with priority
    let prioResults = result.filter(r => !r.completed && !!r.priority);
    prioResults.sort(firstBy("priority"));

    // Retrieve all not-done models without priority
    let noPrioResults = result.filter(r => !r.completed && !r.priority);

    // Retrieve all done models
    let doneModels = result.filter(r => r.completed);

    return prioResults.concat(noPrioResults).concat(doneModels);
}

export function singleTodoTxtToString(model: TodoTxtModel) {
    let result = "";
    if (model.completed) {
        result += "x ";
    }

    if (model.priority) {
        result += `(${model.priority}) `;
    }

    if (model.creationDate) {
        if (model.completionDate) {
            result += `${moment(model.completionDate).format('YYYY-MM-DD')} `;
        }

        result += `${moment(model.creationDate).format('YYYY-MM-DD')} `;
    }

    result += model.description;
    return result;
}

export function todoTxtToString(models: TodoTxtModel[]) {
    let result = [];
    for (let model of models) {
        result.push(singleTodoTxtToString(model));
    }

    return result.join('\n');
}

export function extractProjectTags(models: TodoTxtModel[]): string[] {
    let result: string[] = [];
    models = models.filter(m => !m.completed);
    for (let model of models) {
        for (let tag of model.projectTags) {
            if (result.indexOf(tag) === -1) {
                result.push(tag);
            }
        }
    }

    return result;
}

export function extractContextTags(models: TodoTxtModel[]): string[] {
    let result: string[] = [];
    models = models.filter(m => !m.completed);
    for (let model of models) {
        for (let tag of model.contextTags) {
            if (result.indexOf(tag) === -1) {
                result.push(tag);
            }
        }
    }

    return result;
}