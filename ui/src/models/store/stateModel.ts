import Notebook from '@/models/api/notebook';
import Note from '@/models/api/note';
import { MessageModel } from './messageModel';
import { SignedInModel } from '../api/signedInModel';

export interface StateModel {
    message: MessageModel,
    notebooks: Notebook[],
    notes: Note[]
    signedInUser: SignedInModel
}