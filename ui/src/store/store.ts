import Vue from 'vue';
import Vuex from 'vuex';
import urls from '@/urls';
import axios from 'axios';
import VueAxios from 'vue-axios';
import { StateModel } from '@/models/stateModel';
import Notebook from '@/models/api/notebook';
import Note from '@/models/api/note';

Vue.use(Vuex);
Vue.use(Vuex);
Vue.use(VueAxios, axios);

const state: StateModel = {
  notebooks: [],
  notes: []
};

export default new Vuex.Store({
  state,
  mutations: {
    SET_NOTEBOOKS(state: StateModel, notebooks: Notebook[]) {
      state.notebooks = notebooks;
    },
    SET_NOTES(state: StateModel, notes: Note[]) {
      state.notes = notes;
    }
  },
  actions: {
    loadNotebooks({ commit }) {
      axios.get(`${urls.rootUrl}api/v1/notebooks`)
        .then(r => r.data)
        .then((notebooks: Notebook[]) => {
          commit('SET_NOTEBOOKS', notebooks)
        });
    },
    loadNotes({ commit }, notebookId: number) {
      axios.get(`${urls.rootUrl}api/v1/notebooks/${notebookId}/notes`)
        .then(r => r.data)
        .then((notes: Note[]) => {
          commit('SET_NOTES', notes)
        });
    }
  },
});
