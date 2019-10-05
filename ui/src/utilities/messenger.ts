import store from '@/store/store';
import { MessageModel, MessageType } from '@/models/store/messageModel';

export function infoMessage(message: string) {
    let messageModel: MessageModel = {
        message: message,
        timestamp: new Date().getTime(),
        type: MessageType.INFO
    };
    store.commit('SET_MESSAGE', messageModel);
};

export function successMessage(message: string) {
    let messageModel: MessageModel = {
        message: message,
        timestamp: new Date().getTime(),
        type: MessageType.SUCCESS
    };
    store.commit('SET_MESSAGE', messageModel);
};

export function warningMessage(message: string) {
    let messageModel: MessageModel = {
        message: message,
        timestamp: new Date().getTime(),
        type: MessageType.WARNING
    };
    store.commit('SET_MESSAGE', messageModel);
};

export function errorMessage(message: string) {
    let messageModel: MessageModel = {
        message: message,
        timestamp: new Date().getTime(),
        type: MessageType.ERROR
    };
    store.commit('SET_MESSAGE', messageModel);
};