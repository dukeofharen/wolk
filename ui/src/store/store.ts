import Vue from 'vue';
import Vuex from 'vuex';

import axios from 'axios';
import VueAxios from 'vue-axios';

import { StateModel } from '@/models/store/stateModel';
import { MessageType } from '@/models/store/messageModel';

import { SET_NOTEBOOKS, SET_CURRENT_NOTEBOOK } from '@/store/mutations/notebooks';
import { SET_NOTES, SET_CURRENT_NOTE } from '@/store/mutations/notes';
import { SET_MESSAGE } from '@/store/mutations/general';
import { SET_SIGNED_IN_USER } from '@/store/mutations/users';

import { loadNotebooks, loadNotebook, createNotebook, updateNotebook } from '@/store/actions/notebooks';
import { loadNotes, loadNote, createNote, updateNote } from '@/store/actions/notes';
import { authenticate } from '@/store/actions/users';

import { signedInUser, isSignedIn } from '@/store/getters/users';

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
  currentNotebook: {
    id: 0,
    name: ''
  },
  currentNote: {
    id: 0,
    content: '',
    created: new Date(),
    notebookId: 0,
    preview: '',
    title: '',
    updated: new Date()
  },
  notes: [],
  signedInUser: {
    email: '',
    id: 0,
    token: ''
  }
};

export default new Vuex.Store({
  state,
  mutations: {
    SET_NOTEBOOKS,
    SET_CURRENT_NOTEBOOK,
    SET_NOTES,
    SET_CURRENT_NOTE,
    SET_MESSAGE,
    SET_SIGNED_IN_USER
  },
  actions: {
    loadNotebooks,
    loadNotebook,
    createNotebook,
    updateNotebook,
    loadNotes,
    loadNote,
    createNote,
    updateNote,
    authenticate
  },
  getters: {
    signedInUser,
    isSignedIn
  },
});
