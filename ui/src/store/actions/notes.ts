import { StateModel } from '@/models/store/stateModel';
import { ActionContext } from 'vuex';
import urls from '@/urls';
import axios from 'axios';
import Note from '@/models/api/note';

export function loadNotes({ commit }: ActionContext<StateModel, StateModel>, notebookId: string) {
  let url = !!notebookId ?
    `${urls.rootUrl}api/v1/notebooks/${notebookId}/notes` :
    `${urls.rootUrl}api/v1/notes`;
  axios.get(url)
    .then(r => r.data)
    .then((notes: Note[]) => {
      commit('SET_NOTES', notes)
    });
}