import {ActionContext} from "vuex";
import {StateModel} from "@/models/store/stateModel";
import createInstance from "@/axios/axiosInstanceFactory";
import urls from "@/urls";
import {downloadBlob} from "@/utilities/downloadHelper";
import {successMessage} from "@/utilities/messenger";
import {resources} from "@/resources";

export function downloadBackup({commit}: ActionContext<StateModel, StateModel>) {
    let instance = createInstance();
    instance({
        url: `${urls.rootUrl}api/backup`,
        method: 'GET',
        responseType: 'blob'
    })
        .then(r => r.data)
        .then(data => {
            downloadBlob("wolk-backup.zip", data);
        });
}

export interface UploadBackupCommand {
    zipBytes: string;
}

export function uploadBackup({commit}: ActionContext<StateModel, StateModel>, command: UploadBackupCommand) {
    let instance = createInstance();
    instance.post(`${urls.rootUrl}api/backup`, command)
        .then(r => r.data)
        .then(data => {
           successMessage(resources.backupRestoredSuccessfully);
           commit("UNSET_SIGNED_IN_USER");
        });
}