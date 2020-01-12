import Notebook from '@/models/api/notebook';
import Note from '@/models/api/note';
import { MessageModel } from './messageModel';
import { SignedInModel } from '@/models/api/signedInModel';
import Attachment from '@/models/api/attachment';
import {UiStateModel} from "@/models/store/uiStateModel";
import {EventModel} from "@/models/store/eventModel";
import {TodoTxtFilterModel} from "@/models/store/todoTxtFilterModel";

export interface StateModel {
    message: MessageModel;
    notebooks: Notebook[];
    currentNotebook: Notebook;
    currentNote: Note;
    notes: Note[];
    attachments: Attachment[];
    signedInUser: SignedInModel;
    uiState: UiStateModel;
    attachmentAccessUrl: string;
    pageSubTitle: string;
    event: EventModel;
    todoTxtFilter: TodoTxtFilterModel;
}