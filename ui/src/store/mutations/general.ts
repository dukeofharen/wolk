import { StateModel } from '@/models/store/stateModel';
import { MessageModel } from '@/models/store/messageModel';

export function SET_MESSAGE(state: StateModel, message: MessageModel) {
    if (
        !message.force &&
        state.message.message === message.message &&
        state.message.type === message.type) {
        return;
    }

    state.message = message;
}