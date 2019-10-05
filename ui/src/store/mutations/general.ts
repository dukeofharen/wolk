import { StateModel } from '@/models/store/stateModel';
import { MessageModel } from '@/models/store/messageModel';

export function SET_MESSAGE(state: StateModel, message: MessageModel) {
    state.message = message;
}