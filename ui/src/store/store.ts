import Vue from 'vue';
import Vuex from 'vuex';

import axios from 'axios';
import VueAxios from 'vue-axios';

import {StateModel} from '@/models/store/stateModel';
import {MessageType} from '@/models/store/messageModel';
import {NoteType} from '@/models/api/enums/noteType';

import { constructStore } from '@/store/storeConstructor';

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
        changed: new Date(),
        opened: new Date()
    },
    notes: [],
    attachments: [],
    signedInUser: {
        email: '',
        id: 0,
        token: ''
    },
    uiState: {
        isOnTop: true,
        attachmentDialogOpened: false
    },
    attachmentAccessUrl: "",
    pageSubTitle: "",
    event: {
        key: "",
        timestamp: 0
    }
};

export default new Vuex.Store(constructStore(state));
