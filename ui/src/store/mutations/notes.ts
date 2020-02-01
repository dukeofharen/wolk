import {StateModel} from '@/models/store/stateModel';
import Note from '@/models/api/note';

export function SET_NOTES(state: StateModel, notes: Note[]) {
    state.notes = notes;
}

export function SET_CURRENT_NOTE(state: StateModel, note: Note) {
    state.currentNote = note;
}