import urls from '@/urls';
import axios, { AxiosResponse } from 'axios';
import { ActionContext } from 'vuex';
import { StateModel } from '@/models/store/stateModel';
import Attachment from '@/models/api/attachment';
import { downloadBlob, getDownloadFilename } from "@/utilities/downloadHelper";
import store from '../store';

import { successMessage } from '@/utilities/messenger';
import { resources } from '@/resources';

export function loadAttachments({ commit }: ActionContext<StateModel, StateModel>, noteId: number) {
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
export function downloadAttachment({ commit }: ActionContext<StateModel, StateModel>, query: DownloadAttachmentQuery) {
    axios.get(`${urls.rootUrl}api/note/${query.noteId}/attachments/${query.attachmentId}`, { responseType: 'blob' })
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
export function uploadAttachment({ commit }: ActionContext<StateModel, StateModel>, command: UploadAttachmentCommand) {
    axios.post(`${urls.rootUrl}api/note/${command.noteId}/attachments`, command)
        .then(r => r.data)
        .then((attachment: Attachment) => {
            successMessage(resources.attachmentUploaded.format(attachment.filename));
            store.dispatch("loadAttachments", command.noteId);
        });
}