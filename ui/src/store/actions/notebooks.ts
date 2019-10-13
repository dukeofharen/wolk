import Notebook from '@/models/api/notebook';
import urls from '@/urls';
import axios from 'axios';
import { ActionContext } from 'vuex';
import { StateModel } from '@/models/store/stateModel';
import store from '@/store/store';
import router from '@/router';

import { successMessage } from '@/utilities/messenger';
import { resources } from '@/resources';

export function loadNotebooks({ commit }: ActionContext<StateModel, StateModel>) {
    axios.get(`${urls.rootUrl}api/notebook`)
        .then(r => r.data)
        .then((notebooks: Notebook[]) => {
            commit('SET_NOTEBOOKS', notebooks)
        });
}

export function loadNotebook({ commit }: ActionContext<StateModel, StateModel>, notebookId: string) {
    axios.get(`${urls.rootUrl}api/notebook/${notebookId}`)
        .then(r => r.data)
        .then((notebooks: Notebook[]) => {
            commit('SET_CURRENT_NOTEBOOK', notebooks)
        });
}

export function createNotebook({ commit }: ActionContext<StateModel, StateModel>, notebook: Notebook) {
    axios.post(`${urls.rootUrl}api/notebook`, notebook)
        .then(r => r.data)
        .then((addedNotebook: Notebook) => {
            store.dispatch('loadNotebooks');
            router.push({ name: 'notesList', params: <any>{ id: addedNotebook.id } });
            successMessage(resources.notebookCreated);
        });
}

interface UpdateNotebookInput {
    id: number;
    notebook: Notebook;
}
export function updateNotebook({ commit }: ActionContext<StateModel, StateModel>, input: UpdateNotebookInput) {
    axios.put(`${urls.rootUrl}api/notebook/${input.id}`, input.notebook)
        .then(r => r.data)
        .then(() => {
            store.dispatch('loadNotebooks');
            router.push({ name: 'notesList', params: <any>{ id: input.id } });
            successMessage(resources.notebookUpdated);
        });
}