import { StateModel } from '@/models/store/stateModel';
import { ActionContext } from 'vuex';
import urls from '@/urls';
import axios from 'axios';
import Note from '@/models/api/note';
import { successMessage } from '@/utilities/messenger';
import { resources } from '@/resources';

import router from '@/router';
import { LoadNotesQueryModel } from '@/models/store/loadNotesQueryModel';

export function loadNotes({ commit }: ActionContext<StateModel, StateModel>, queryModel?: LoadNotesQueryModel) {
  let url = `${urls.rootUrl}api/note`;
  let promise = !!queryModel ? axios.get(url, {
    params: queryModel
  }) : axios.get(url);
    promise.then(r => r.data)
    .then((notes: Note[]) => {
      commit('SET_NOTES', notes)
    });
}

export function loadNote({ commit }: ActionContext<StateModel, StateModel>, noteId: number) {
  let url = `${urls.rootUrl}api/note/${noteId}`;
  axios.get(url)
    .then(r => r.data)
    .then((note: Note) => {
      commit('SET_CURRENT_NOTE', note)
    });
}

export function createNote({ commit }: ActionContext<StateModel, StateModel>, note: Note) {
  axios.post(`${urls.rootUrl}api/note`, note)
    .then(r => r.data)
    .then((addedNote: Note) => {
      router.push({ name: 'noteForm', params: <any>{ id: addedNote.id } });
      successMessage(resources.noteCreated);
    });
}

interface UpdateNoteInput {
  id: number;
  note: Note;
}
export function updateNote({ commit }: ActionContext<StateModel, StateModel>, input: UpdateNoteInput) {
  axios.put(`${urls.rootUrl}api/note/${input.id}`, input.note)
    .then(r => r.data)
    .then(() => {
      successMessage(resources.noteUpdated);
    });
}

export function deleteNote({ commit }: ActionContext<StateModel, StateModel>, note: Note) {
  axios.delete(`${urls.rootUrl}api/note/${note.id}`)
    .then(r => r.data)
    .then(() => {
      router.push({ name: 'notesList', params: <any>{ id: note.notebookId } });
      successMessage(resources.noteDeleted);
    });
}