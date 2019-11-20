import {StateModel} from "@/models/store/stateModel";

export function SET_IS_ON_TOP(state: StateModel, isOnTop: boolean) {
    state.uiState.isOnTop = isOnTop
}