import { StateModel } from '@/models/stateModel';
import { ActionContext } from 'vuex';
import urls from '@/urls';
import axios from 'axios';
import Note from '@/models/api/note';

export function loadNotes({ commit }: ActionContext<StateModel, StateModel>, notebookId: string) {
    axios.get(`${urls.rootUrl}api/v1/notebooks/${notebookId}/notes`)
      .then(r => r.data)
      .then((notes: Note[]) => {
        commit('SET_NOTES', notes)
      });
  }