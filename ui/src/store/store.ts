import Vue from 'vue';
import Vuex from 'vuex';
import urls from '@/urls';
import axios from 'axios';
import VueAxios from 'vue-axios';
import { StateModel } from '@/models/stateModel';
import Notebook from '@/models/api/notebook';

Vue.use(Vuex);
Vue.use(Vuex);
Vue.use(VueAxios, axios);

const state: StateModel = {
  notebooks: []
};

export default new Vuex.Store({
  state,
  mutations: {
    SET_NOTEBOOKS(state: StateModel, notebooks: Notebook[]) {
      state.notebooks = notebooks;
    }
  },
  actions: {
    loadNotebooks({ commit }) {
      axios.get(`${urls.rootUrl}api/v1/notebooks`)
        .then(r => r.data)
        .then((notebooks: Notebook[]) => {
          commit('SET_NOTEBOOKS', notebooks)
        })
    }
  },
});
