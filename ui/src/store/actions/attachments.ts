import urls from '@/urls';
import axios, {AxiosResponse} from 'axios';
import {ActionContext} from 'vuex';
import {StateModel} from '@/models/store/stateModel';
import Attachment from '@/models/api/attachment';
import {downloadBlob, getDownloadFilename} from "@/utilities/downloadHelper";
import store from '../store';

import {successMessage} from '@/utilities/messenger';
import {resources} from '@/resources';
import {AccessTokenResultModel} from "@/models/api/accessTokenResultModel";

export function loadAttachments({commit}: ActionContext<StateModel, StateModel>, noteId: number) {
    axios.get(`${urls.rootUrl}api/note/${noteId}/attachments`)
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
    axios.get(`${urls.rootUrl}api/note/${query.noteId}/attachments/${query.attachmentId}`, {responseType: 'blob'})
        .then((response: AxiosResponse<any>) => {
            let attachment = response.data;
            let filename = getDownloadFilename(response.headers["content-disposition"]);

            downloadBlob(filename, attachment);
        });
}

export interface UploadAttachmentCommand {
    noteId: number;
    filename: string;
    base64Contents: string;
}

export function uploadAttachment({commit}: ActionContext<StateModel, StateModel>, command: UploadAttachmentCommand) {
    axios.post(`${urls.rootUrl}api/note/${command.noteId}/attachments`, command)
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
    axios.delete(`${urls.rootUrl}api/note/${command.noteId}/attachments/${command.attachmentId}`)
        .then(r => r.data)
        .then(() => {
            successMessage(resources.attachmentDeleted);
            store.dispatch("loadAttachments", command.noteId);
        })
}

export interface CreateAttachmentAccessTokenCommand {
    noteId: number;
    attachmentId: number;
    expirationDateTime: Date;
}

export function createAttachmentAccessToken({commit}: ActionContext<StateModel, StateModel>, command: CreateAttachmentAccessTokenCommand) {
    axios.post(`${urls.rootUrl}api/note/${command.noteId}/attachments/${command.attachmentId}/accessTokens`)
        .then(r => r.data)
        .then((result: AccessTokenResultModel) => {
            console.log(result);
        });
}