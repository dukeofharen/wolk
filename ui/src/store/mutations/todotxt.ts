import {StateModel} from "@/models/store/stateModel";
import {TodoTxtFilterModel} from "@/models/store/todoTxtFilterModel";

export function SET_TODOTXT_FILTER(state: StateModel, filter: TodoTxtFilterModel) {
    state.todoTxtFilter = filter;
}