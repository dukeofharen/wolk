import {StateModel} from '@/models/store/stateModel';
import Note from '@/models/api/note';

export function SET_NOTES(state: StateModel, notes: Note[]) {
    notes.sort((a: Note, b: Note) => {
        if (!a.opened) {
            return 1;
        }

        if (a.opened && !b.opened) {
            return 1;
        }

        if (b.opened && new Date(a.opened).getTime() < new Date(b.opened).getTime()) {
            return 1;
        }

        return 0;
    });
    state.notes = notes;
}

export function SET_CURRENT_NOTE(state: StateModel, note: Note) {
    state.currentNote = note;
}