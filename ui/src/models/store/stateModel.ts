import Notebook from '@/models/api/notebook';
import Note from '@/models/api/note';
import { MessageModel } from './messageModel';

export interface StateModel {
    message: MessageModel,
    notebooks: Notebook[],
    notes: Note[]
}