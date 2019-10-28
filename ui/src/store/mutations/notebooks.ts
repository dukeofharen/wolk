import { StateModel } from '@/models/store/stateModel';
import Notebook from '@/models/api/notebook';

export function SET_NOTEBOOKS(state: StateModel, notebooks: Notebook[]) {
    notebooks.sort((a: Notebook, b: Notebook) => {
        let textA = a.name.toUpperCase();
        let textB = b.name.toUpperCase();
        return (textA < textB) ? -1 : (textA > textB) ? 1 : 0;
    });
    state.notebooks = notebooks;
}

export function SET_CURRENT_NOTEBOOK(state: StateModel, notebook: Notebook) {
    state.currentNotebook = notebook;
}