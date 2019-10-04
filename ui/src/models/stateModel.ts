import Notebook from '@/models/api/notebook';
import Note from './api/note';

export interface StateModel {
    notebooks: Notebook[],
    notes: Note[]
}