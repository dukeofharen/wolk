import { StateModel } from '@/models/stateModel';
import Note from '@/models/api/note';

export function SET_NOTES(state: StateModel, notes: Note[]) {
    state.notes = notes;
}