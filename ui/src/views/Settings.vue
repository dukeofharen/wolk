<template>
    <v-row>
        <v-col>
            <h1>Settings</h1>
            <v-card>
                <v-card-title>Backup / restore</v-card-title>
                <v-card-text>
                    <p>
                        Here, you can download a backup of the complete Wolk database as .zip file. This contains all
                        entities that are in the database (users, notes, notebooks, attachments etc.)
                    </p>
                    <v-btn color="primary" @click="downloadBackup">Download backup</v-btn>

                    <p>
                        Here, you can upload an existing backup from your PC. <strong>ATTENTION!</strong> Everything in
                        the current Wolk instance will be overwritten! You need to log in again after restoring a
                        backup.
                    </p>
                    <input
                            type="file"
                            name="file"
                            ref="fileUpload"
                            @change="loadFromFile"
                    />
                    <v-btn color="primary" @click="uploadBackup">Upload backup</v-btn>
                </v-card-text>
            </v-card>
        </v-col>
    </v-row>
</template>

<script lang="ts">
    import {Component, Vue} from "vue-property-decorator";
    import OverviewNote from "@/components/OverviewNote.vue";
    import BackToTop from "@/components/BackToTop.vue";
    import {UploadBackupCommand} from "@/store/actions/backups";

    @Component({
        components: {OverviewNote, BackToTop}
    })
    export default class Overview extends Vue {
        constructor() {
            super();
        }

        mounted() {
            this.$store.commit("SET_PAGE_SUB_TITLE", "Settings");
        }

        downloadBackup() {
            this.$store.dispatch("downloadBackup");
        }

        uploadBackup() {
            (<any>this.$refs.fileUpload).click();
        }

        loadFromFile(ev: any) {
            let file = ev.target.files[0];
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = e => {
                if (!e || !e.target) {
                    return;
                }

                let dataUrl: string = e.target.result as string;
                let content = dataUrl.split(",")[1];
                let command: UploadBackupCommand = {
                    zipBytes: content
                };
                this.$store.dispatch("uploadBackup", command);
            };
        }
    }
</script>

<style scoped>
</style>