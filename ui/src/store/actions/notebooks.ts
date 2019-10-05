import Notebook from '@/models/api/notebook';
import urls from '@/urls';
import axios from 'axios';
import { ActionContext } from 'vuex';
import { StateModel } from '@/models/store/stateModel';

export function loadNotebooks({ commit }: ActionContext<StateModel, StateModel>) {
    axios.get(`${urls.rootUrl}api/v1/notebooks`)
        .then(r => r.data)
        .then((notebooks: Notebook[]) => {
            commit('SET_NOTEBOOKS', notebooks)
        });
}