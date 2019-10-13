import { StateModel } from '@/models/store/stateModel';
import { ActionContext } from 'vuex';
import urls from '@/urls';
import axios from 'axios';
import Note from '@/models/api/note';
import { successMessage } from '@/utilities/messenger';
import { resources } from '@/resources';

export function loadNotes({ commit }: ActionContext<StateModel, StateModel>, notebookId: string) {
  let url = !!notebookId ?
    `${urls.rootUrl}api/notebook/${notebookId}/notes` :
    `${urls.rootUrl}api/note`;
  axios.get(url)
    .then(r => r.data)
    .then((notes: Note[]) => {
      commit('SET_NOTES', notes)
    });
}

export function createNote({ commit }: ActionContext<StateModel, StateModel>, note: Note) {
  axios.post(`${urls.rootUrl}api/note`, note)
      .then(r => r.data)
      .then((addedNote: Note) => {
          // store.dispatch('loadNotebooks');
          // router.push({ name: 'notesList', params: <any>{ id: addedNotebook.id } });
          // TODO redirect to update page
          successMessage(resources.noteCreated);
      });
}