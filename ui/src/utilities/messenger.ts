import store from '@/store/store';
import { MessageModel, MessageType } from '@/models/store/messageModel';

export function infoMessage(message: string, force: boolean = true) {
    let messageModel: MessageModel = {
        message: message,
        timestamp: new Date().getTime(),
        type: MessageType.INFO,
        force: force
    };
    store.commit('SET_MESSAGE', messageModel);
};

export function successMessage(message: string, force: boolean = true) {
    let messageModel: MessageModel = {
        message: message,
        timestamp: new Date().getTime(),
        type: MessageType.SUCCESS,
        force: force
    };
    store.commit('SET_MESSAGE', messageModel);
};

export function warningMessage(message: string, force: boolean = true) {
    let messageModel: MessageModel = {
        message: message,
        timestamp: new Date().getTime(),
        type: MessageType.WARNING,
        force: force
    };
    store.commit('SET_MESSAGE', messageModel);
};

export function errorMessage(message: string, force: boolean = true) {
    let messageModel: MessageModel = {
        message: message,
        timestamp: new Date().getTime(),
        type: MessageType.ERROR,
        force: force
    };
    store.commit('SET_MESSAGE', messageModel);
};