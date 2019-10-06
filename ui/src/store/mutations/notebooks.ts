import { StateModel } from '@/models/store/stateModel';
import Notebook from '@/models/api/notebook';

export function SET_NOTEBOOKS(state: StateModel, notebooks: Notebook[]) {
    state.notebooks = notebooks;
};

export function SET_CURRENT_NOTEBOOK(state: StateModel, notebook: Notebook) {
    state.currentNotebook = notebook;
};