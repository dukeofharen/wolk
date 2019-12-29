import Notebook from '@/models/api/notebook';
import urls from '@/urls';
import createInstance from '@/axios/axiosInstanceFactory';
import { ActionContext } from 'vuex';
import { StateModel } from '@/models/store/stateModel';
import store from '@/store/store';
import router from '@/router';

import { successMessage } from '@/utilities/messenger';
import { resources } from '@/resources';

export function loadNotebooks({ commit }: ActionContext<StateModel, StateModel>) {
    let instance = createInstance();
    instance.get(`${urls.rootUrl}api/notebook`)
        .then(r => r.data)
        .then((notebooks: Notebook[]) => {
            commit('SET_NOTEBOOKS', notebooks)
        });
}

export function loadNotebook({ commit }: ActionContext<StateModel, StateModel>, notebookId: string) {
    let instance = createInstance();
    instance.get(`${urls.rootUrl}api/notebook/${notebookId}`)
        .then(r => r.data)
        .then((notebooks: Notebook[]) => {
            commit('SET_CURRENT_NOTEBOOK', notebooks)
        });
}

export function createNotebook({ commit }: ActionContext<StateModel, StateModel>, notebook: Notebook) {
    let instance = createInstance();
    instance.post(`${urls.rootUrl}api/notebook`, notebook)
        .then(r => r.data)
        .then((addedNotebook: Notebook) => {
            store.dispatch('loadNotebooks');
            router.push({ name: 'notesList', params: <any>{ id: addedNotebook.id } });
            successMessage(resources.notebookCreated);
        });
}

export interface UpdateNotebookCommand {
    id: number;
    notebook: Notebook;
}
export function updateNotebook({ commit }: ActionContext<StateModel, StateModel>, command: UpdateNotebookCommand) {
    let instance = createInstance();
    instance.put(`${urls.rootUrl}api/notebook/${command.id}`, command.notebook)
        .then(r => r.data)
        .then(() => {
            store.dispatch('loadNotebooks');
            router.push({ name: 'notesList', params: <any>{ id: command.id } });
            successMessage(resources.notebookUpdated);
        });
}

export function deleteNotebook({ commit }: ActionContext<StateModel, StateModel>, id: number) {
    let instance = createInstance();
    instance.delete(`${urls.rootUrl}api/notebook/${id}`)
        .then(r => r.data)
        .then(() => {
            router.push({ name: 'overview' });
            store.dispatch('loadNotebooks');
            successMessage(resources.notebookDeleted);
        });
}