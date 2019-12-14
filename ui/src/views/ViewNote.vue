<template>
    <v-row>
        <v-col>
            <h1>{{note.title}}</h1>
            <div>
                <v-chip title="Date/time created">
                    <v-icon left>mdi-clock</v-icon>
                    {{note.created | datetime}}
                </v-chip>
                <v-chip
                        title="Date/time updated"
                        v-if="note.changed"
                >
                    <v-icon left>mdi-clock</v-icon>
                    {{note.changed | datetime}}
                </v-chip>
            </div>
            <v-dialog scrollable v-model="uiState.attachmentDialogOpened">
                <Attachments :noteId="note.id"/>
            </v-dialog>
            <NoteRender
                    :contents="note.content"
                    :noteType="note.noteType"
            />
            <v-bottom-navigation color="indigo" fixed>
                <v-btn title="Update note" :to="{ name: 'noteForm', params: {id: note.id}}">
                    <v-icon>mdi-lead-pencil</v-icon>
                </v-btn>
                <v-btn title="Reload note" @click="reloadData">
                    <v-icon>mdi-refresh</v-icon>
                </v-btn>
                <v-btn title="Attachments" @click="showAttachments" >
                    <v-icon>mdi-paperclip</v-icon>
                </v-btn>
                <v-btn title="Delete note" @click="deleteNote">
                    <v-icon>mdi-delete</v-icon>
                </v-btn>
            </v-bottom-navigation>
        </v-col>
    </v-row>
</template>

<script lang="ts">
    import {mapState} from "vuex";
    import {Component, Vue, Watch} from "vue-property-decorator";
    import Note from "../models/api/note";
    import {resources} from "@/resources";
    import {NoteType} from "@/models/api/enums/noteType";
    import NoteRender from "@/components/NoteRender.vue";
    import Attachments from "@/components/Attachments.vue";
    import {UiStateModel} from "@/models/store/uiStateModel";

    @Component({
        components: {NoteRender, Attachments},
        computed: mapState(["currentNote", "uiState"])
    })
    export default class ViewNote extends Vue {
        NoteType = NoteType;

        attachmentsOpened: boolean = false;
        note: Note = {
            id: 0,
            title: "",
            content: "",
            notebookId: 0,
            preview: "",
            noteType: NoteType.NotSet,
            created: new Date(),
            updated: new Date(),
            opened: new Date()
        };
        uiState!: UiStateModel;

        constructor() {
            super();
        }

        mounted() {
            this.reloadData();
        }

        @Watch("currentNote")
        onCurrentNoteChanged(value: Note) {
            this.note = value;
        }

        @Watch("$route")
        onRouteChanged() {
            this.reloadData();
        }

        updateNote() {
            this.$router.push({
                name: "noteForm",
                params: <any>{id: this.note.id}
            });
        }

        deleteNote() {
            if (confirm(resources.areYouSureDeleteNote)) {
                this.$store.dispatch("deleteNote", this.note);
            }
        }

        showAttachments() {
            this.uiState.attachmentDialogOpened = !this.uiState.attachmentDialogOpened;
        }

        private reloadData() {
            this.$store.dispatch("loadNote", this.$route.params.id);
        }
    }
</script>