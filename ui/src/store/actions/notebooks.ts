import Notebook from '@/models/api/notebook';
import urls from '@/urls';
import axios from 'axios';
import { ActionContext } from 'vuex';
import { StateModel } from '@/models/store/stateModel';

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
            // TODO redirect to somewhere
            // TODO re-retrieve notebooks
            successMessage(resources.notebookCreated);
        });
}