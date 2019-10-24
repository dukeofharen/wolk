import Vue from 'vue';
import Vuex from 'vuex';

import axios from 'axios';
import VueAxios from 'vue-axios';

import { StateModel } from '@/models/store/stateModel';
import { MessageType } from '@/models/store/messageModel';

import { SET_ATTACHMENTS } from '@/store/mutations/attachments';
import { SET_NOTEBOOKS, SET_CURRENT_NOTEBOOK } from '@/store/mutations/notebooks';
import { SET_NOTES, SET_CURRENT_NOTE } from '@/store/mutations/notes';
import { SET_MESSAGE } from '@/store/mutations/general';
import { SET_SIGNED_IN_USER, UNSET_SIGNED_IN_USER } from '@/store/mutations/users';

import { loadAttachments, downloadAttachment, uploadAttachment } from '@/store/actions/attachments';
import { loadNotebooks, loadNotebook, createNotebook, updateNotebook, deleteNotebook } from '@/store/actions/notebooks';
import { loadNotes, loadNote, createNote, updateNote, deleteNote } from '@/store/actions/notes';
import { authenticate } from '@/store/actions/users';

import { signedInUser, isSignedIn } from '@/store/getters/users';
import { NoteType } from '@/models/api/enums/noteType';

Vue.use(Vuex);
Vue.use(Vuex);
Vue.use(VueAxios, axios);

const state: StateModel = {
  message: {
    message: '',
    timestamp: 0,
    type: MessageType.NOT_SET,
    force: true
  },
  notebooks: [],
  currentNotebook: {
    id: 0,
    name: '',
    created: new Date(),
    updated: new Date()
  },
  currentNote: {
    id: 0,
    content: '',
    created: new Date(),
    notebookId: 0,
    preview: '',
    title: '',
    noteType: NoteType.NotSet,
    updated: new Date()
  },
  notes: [],
  attachments: [],
  signedInUser: {
    email: '',
    id: 0,
    token: ''
  }
};

export default new Vuex.Store({
  state,
  mutations: {
    SET_ATTACHMENTS,
    SET_NOTEBOOKS,
    SET_CURRENT_NOTEBOOK,
    SET_NOTES,
    SET_CURRENT_NOTE,
    SET_MESSAGE,
    SET_SIGNED_IN_USER,
    UNSET_SIGNED_IN_USER
  },
  actions: {
    loadAttachments,
    downloadAttachment,
    uploadAttachment,
    loadNotebooks,
    loadNotebook,
    createNotebook,
    updateNotebook,
    deleteNotebook,
    loadNotes,
    loadNote,
    createNote,
    updateNote,
    deleteNote,
    authenticate
  },
  getters: {
    signedInUser,
    isSignedIn
  },
});
