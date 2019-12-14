import store from '@/store/store';
import {MessageModel, MessageType} from '@/models/store/messageModel';

export function infoMessage(
    message: string | string[],
    force: boolean = true,
    callback?: object) {
    let messageModel: MessageModel = {
        message: message,
        timestamp: new Date().getTime(),
        type: MessageType.INFO,
        force: force,
        callback: callback
    };
    store.commit('SET_MESSAGE', messageModel);
};

export function successMessage(
    message: string | string[],
    force: boolean = true,
    callback?: object) {
    let messageModel: MessageModel = {
        message: message,
        timestamp: new Date().getTime(),
        type: MessageType.SUCCESS,
        force: force,
        callback: callback
    };
    store.commit('SET_MESSAGE', messageModel);
};

export function warningMessage(
    message: string | string[],
    force: boolean = true,
    callback?: object) {
    let messageModel: MessageModel = {
        message: message,
        timestamp: new Date().getTime(),
        type: MessageType.WARNING,
        force: force,
        callback: callback
    };
    store.commit('SET_MESSAGE', messageModel);
};

export function errorMessage(
    message: string | string[],
    force: boolean = true,
    callback?: object) {
    let messageModel: MessageModel = {
        message: message,
        timestamp: new Date().getTime(),
        type: MessageType.ERROR,
        force: force,
        callback: callback
    };
    store.commit('SET_MESSAGE', messageModel);
};