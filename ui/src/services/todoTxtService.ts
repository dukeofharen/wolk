import {TodoTxtModel} from "@/models/todoTxtModel";
import {firstBy} from "thenby";

export function todoTxtToModels(input: string): TodoTxtModel[] {
    let result: TodoTxtModel[] = [];
    let lines = input.lines();
    for (let line of lines) {
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

        result.push(model);
    }

    result.sort(firstBy("completed").thenBy(<any>"priority"));

    return result;
}