import {StateModel} from "@/models/store/stateModel";
import {TodoTxtFilterModel} from "@/models/store/todoTxtFilterModel";

export function todoTxtFilter(state: StateModel): TodoTxtFilterModel {
    return state.todoTxtFilter;
}