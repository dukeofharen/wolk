<template>
    <v-card>
        <v-card-title class="headline grey lighten-2">Attachments</v-card-title>
        <v-divider/>
        <v-card-text>
            <v-list>
                <v-list-item-group>
                    <v-list-item v-for="attachment of attachments" :key="attachment.id">
                        <v-list-item-icon>
                            <v-icon @click="deleteAttachment(attachment)" title="Delete attachment">mdi-delete</v-icon>
                            <v-icon @click="getAttachmentLink(attachment)"
                                    title="Get link for sharing or inserting attachment">mdi-link
                            </v-icon>
                        </v-list-item-icon>
                        <v-list-item-content @click="openAttachment(attachment)">
                            <v-list-item-title>
                                {{attachment.filename}} ({{attachment.fileSize | filesize}})
                            </v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>
                </v-list-item-group>
            </v-list>
        </v-card-text>
        <v-card-actions>
            <v-btn
                    title="Upload attachment"
                    @click="uploadAttachment"
                    color="success"
            ><v-icon>mdi-cloud-upload</v-icon>
            </v-btn>
            <v-btn @click="closeClick" title="Close dialog"><v-icon>mdi-close</v-icon></v-btn>
            <input
                    type="file"
                    name="file"
                    ref="fileUpload"
                    @change="loadFromFile"
                    multiple
            />
        </v-card-actions>
    </v-card>
</template>
<script lang="ts">
    import {mapState} from "vuex";
    import {Component, Vue, Prop, Watch} from "vue-property-decorator";
    import Attachment from "../models/api/attachment";
    import {
        DownloadAttachmentQuery,
        UploadAttachmentCommand,
        DeleteAttachmentCommand, CreateAttachmentAccessTokenCommand
    } from "@/store/actions/attachments";
    import {resources} from "@/resources";
    import {UiStateModel} from "@/models/store/uiStateModel";
    import {successMessage} from "@/utilities/messenger";
    import {clipboardCopy} from "@/utilities/clipboardHelper";

    @Component({
        components: {},
        computed: mapState(["attachments", "uiState", "attachmentAccessUrl"])
    })
    export default class Attachments extends Vue {
        attachments!: Attachment[];
        uiState!: UiStateModel;
        attachmentAccessUrl!: string;

        @Prop()
        noteId!: number;

        constructor() {
            super();
            this.reloadData();
        }

        openAttachment(attachment: Attachment) {
            let query: DownloadAttachmentQuery = {
                noteId: attachment.noteId,
                attachmentId: attachment.id
            };
            this.$store.dispatch("downloadAttachment", query);
        }

        deleteAttachment(attachment: Attachment) {
            if (confirm(resources.areYouSureDeleteAttachment)) {
                let command: DeleteAttachmentCommand = {
                    noteId: this.noteId,
                    attachmentId: attachment.id
                };
                this.$store.dispatch("deleteAttachment", command);
            }
        }

        uploadAttachment() {
            (<any>this.$refs.fileUpload).click();
        }

        loadFromFile(ev: any) {
            for (let file of ev.target.files) {
                const reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = e => {
                    if (!e || !e.target) {
                        return;
                    }

                    let dataUrl: string = e.target.result as string;
                    let content = dataUrl.split(",")[1];
                    let command: UploadAttachmentCommand = {
                        noteId: this.noteId,
                        filename: file.name,
                        base64Contents: content
                    };
                    this.$store.dispatch("uploadAttachment", command);
                };
            }
        }

        closeClick() {
            this.uiState.attachmentDialogOpened = !this.uiState.attachmentDialogOpened;
        }

        getAttachmentLink(attachment: Attachment) {
            let command: CreateAttachmentAccessTokenCommand = {
                attachmentId: attachment.id,
                expirationDateTime: undefined,
                noteId: attachment.noteId
            };
            this.$store.dispatch("createAttachmentAccessToken", command);
        }

        @Watch("attachmentAccessUrl")
        onCurrentNoteChanged(value: string) {
            if (value) {
                successMessage(
                    [resources.attachmentAccessCreated.format(value), resources.attachmentAccessClickToCopy],
                true,
                    () => clipboardCopy(value));
            }
        }

        private reloadData() {
            this.$store.dispatch("loadAttachments", this.$route.params.id);
        }
    }
</script>

<style scoped>
</style>