import Vue from 'vue';
import Vuex from 'vuex';

import axios from 'axios';
import VueAxios from 'vue-axios';
import { StateModel } from '@/models/store/stateModel';
import { SET_NOTEBOOKS } from '@/store/mutations/notebooks';
import { SET_NOTES } from '@/store/mutations/notes';
import { SET_MESSAGE } from '@/store/mutations/general';
import { loadNotebooks } from '@/store/actions/notebooks';
import { loadNotes } from '@/store/actions/notes';
import { MessageType } from '@/models/store/messageModel';

Vue.use(Vuex);
Vue.use(Vuex);
Vue.use(VueAxios, axios);

const state: StateModel = {
  message: {
    message: '',
    timestamp: 0,
    type: MessageType.NOT_SET
  },
  notebooks: [],
  notes: []
};

export default new Vuex.Store({
  state,
  mutations: {
    SET_NOTEBOOKS,
    SET_NOTES,
    SET_MESSAGE
  },
  actions: {
    loadNotebooks,
    loadNotes
  },
});
