import urls from '@/urls';
import {AxiosResponse} from 'axios';
import {ActionContext} from 'vuex';
import {StateModel} from '@/models/store/stateModel';
import Attachment from '@/models/api/attachment';
import store from '../store';
import createInstance from '@/axios/axiosInstanceFactory';

import {successMessage} from '@/utilities/messenger';
import {resources} from '@/resources';
import {AccessTokenResultModel} from "@/models/api/accessTokenResultModel";

export function loadAttachments({commit}: ActionContext<StateModel, StateModel>, noteId: number) {
    let instance = createInstance();
    instance.get(`${urls.rootUrl}api/note/${noteId}/attachments`)
        .then(r => r.data)
        .then((attachments: Attachment[]) => {
            commit('SET_ATTACHMENTS', attachments)
        });
}

export interface DownloadAttachmentQuery {
    noteId: number;
    attachmentId: number;
}

export function downloadAttachment({commit}: ActionContext<StateModel, StateModel>, query: DownloadAttachmentQuery) {
    let now = new Date();
    let expiry = new Date(now.getTime() + 60000);
    let command: CreateAttachmentAccessTokenCommand = {
        attachmentId: query.attachmentId,
        expirationDateTime: expiry,
        noteId: query.noteId
    };
    let instance = createInstance();
    instance.post(`${urls.rootUrl}api/note/${command.noteId}/attachments/${command.attachmentId}/accessTokens`, command)
        .then((response: AxiosResponse<AccessTokenResultModel>) => {
            document.location = response.headers['location'];
        });
}

export interface UploadAttachmentCommand {
    noteId: number;
    filename: string;
    base64Contents: string;
}

export function uploadAttachment({commit}: ActionContext<StateModel, StateModel>, command: UploadAttachmentCommand) {
    let instance = createInstance();
    instance.post(`${urls.rootUrl}api/note/${command.noteId}/attachments`, command)
        .then(r => r.data)
        .then((attachment: Attachment) => {
            successMessage(resources.attachmentUploaded.format(attachment.filename));
            store.dispatch("loadAttachments", command.noteId);
        });
}

export interface DeleteAttachmentCommand {
    noteId: number;
    attachmentId: number;
}

export function deleteAttachment({commit}: ActionContext<StateModel, StateModel>, command: DeleteAttachmentCommand) {
    let instance = createInstance();
    instance.delete(`${urls.rootUrl}api/note/${command.noteId}/attachments/${command.attachmentId}`)
        .then(r => r.data)
        .then(() => {
            successMessage(resources.attachmentDeleted);
            store.dispatch("loadAttachments", command.noteId);
        })
}

export interface CreateAttachmentAccessTokenCommand {
    noteId: number;
    attachmentId: number;
    expirationDateTime?: Date;
}

export function createAttachmentAccessToken({commit}: ActionContext<StateModel, StateModel>, command: CreateAttachmentAccessTokenCommand) {
    let instance = createInstance();
    instance.post(`${urls.rootUrl}api/note/${command.noteId}/attachments/${command.attachmentId}/accessTokens`, command)
        .then((response: AxiosResponse<AccessTokenResultModel>) => {
            store.state.attachmentAccessUrl = response.headers['location'];
        });
}