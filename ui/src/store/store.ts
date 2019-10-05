import Vue from 'vue';
import Vuex from 'vuex';

import axios from 'axios';
import VueAxios from 'vue-axios';
import { StateModel } from '@/models/stateModel';
import { SET_NOTEBOOKS } from '@/store/mutations/notebooks';
import { SET_NOTES } from '@/store/mutations/notes';
import { loadNotebooks } from '@/store/actions/notebooks';
import { loadNotes } from '@/store/actions/notes';

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
    SET_NOTEBOOKS,
    SET_NOTES
  },
  actions: {
    loadNotebooks,
    loadNotes
  },
});
