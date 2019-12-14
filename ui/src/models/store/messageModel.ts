export enum MessageType {
    NOT_SET,
    INFO,
    SUCCESS,
    WARNING,
    ERROR
}

export interface MessageModel {
    message: string | string[];
    type: MessageType;
    timestamp: number;
    force: boolean;
    callback?: object;
}