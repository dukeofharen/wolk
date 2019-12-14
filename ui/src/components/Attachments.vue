<template>
    <v-card>
        <v-card-title class="headline grey lighten-2">Attachments</v-card-title>
        <v-divider/>
        <v-card-text>
            <v-list>
                <v-list-item-group>
                    <v-list-item v-for="attachment of attachments" :key="attachment.id">
                        <v-list-item-icon>
                            <v-icon @click="deleteAttachment(attachment)">mdi-delete</v-icon>
                        </v-list-item-icon>
                        <v-list-item-content @click="openAttachment(attachment)">
                            <v-list-item-title>
                                {{attachment.filename}} ({{attachment.fileSize | filesize}})
                            </v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>
                </v-list-item-group>
            </v-list>
            <!--            <v-chip-->
            <!--                    class="ma-2"-->
            <!--                    color="indigo"-->
            <!--                    text-color="white"-->
            <!--                    close-->
            <!--                    close-icon="mdi-delete"-->
            <!--                    v-for="attachment of attachments"-->
            <!--                    :key="attachment.id"-->
            <!--                    @click="openAttachment(attachment)"-->
            <!--                    @click:close="deleteAttachment(attachment)"-->
            <!--            >-->
            <!--                <v-avatar left>-->
            <!--                    <v-icon>mdi-file</v-icon>-->
            <!--                </v-avatar>-->
            <!--                {{attachment.filename}} ({{attachment.fileSize | filesize}})-->
            <!--            </v-chip>-->
        </v-card-text>
        <v-card-actions>
            <v-btn
                    title="Upload attachment"
                    @click="uploadAttachment"
                    color="success"
            >Upload attachment
            </v-btn>
            <v-btn @click="closeClick">Close</v-btn>
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
    import {Component, Vue, Prop} from "vue-property-decorator";
    import Attachment from "../models/api/attachment";
    import {
        DownloadAttachmentQuery,
        UploadAttachmentCommand,
        DeleteAttachmentCommand
    } from "@/store/actions/attachments";
    import {resources} from "@/resources";
    import {UiStateModel} from "@/models/store/uiStateModel";

    @Component({
        components: {},
        computed: mapState(["attachments", "uiState"])
    })
    export default class Attachments extends Vue {
        attachments!: Attachment[];
        uiState!: UiStateModel;

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

        private reloadData() {
            this.$store.dispatch("loadAttachments", this.$route.params.id);
        }
    }
</script>

<style scoped>
</style>