<template>
    <v-row>
        <v-col>
            <transition name="fade">
                <h1 v-if="note.title">{{note.title}}</h1>
            </transition>
            <div>
                <transition name="fade">
                    <v-chip title="Date/time created" v-if="note.created">
                        <v-icon left>mdi-clock</v-icon>
                        {{note.created | datetime}}
                    </v-chip>
                </transition>
                <transition name="fade">
                    <v-chip
                            title="Date/time updated"
                            v-if="note.changed"
                    >
                        <v-icon left>mdi-clock</v-icon>
                        {{note.changed | datetime}}
                    </v-chip>
                </transition>
            </div>
            <v-dialog scrollable v-model="uiState.attachmentDialogOpened">
                <Attachments :noteId="note.id"/>
            </v-dialog>
            <transition name="fade">
                <NoteRender
                        v-if="note.content"
                        :contents="note.content"
                        :noteType="note.noteType"
                        :note="note"
                />
            </transition>
            <v-bottom-navigation color="indigo" fixed>
                <v-btn title="Update note" :to="{ name: 'noteForm', params: {id: note.id}}">
                    <v-icon>mdi-lead-pencil</v-icon>
                </v-btn>
                <v-btn title="Reload note" @click="reloadData">
                    <v-icon>mdi-refresh</v-icon>
                </v-btn>
                <v-btn title="Attachments" @click="showAttachments">
                    <v-icon>mdi-paperclip</v-icon>
                </v-btn>
                <v-btn title="Delete note" @click="deleteNote" color="error">
                    <v-icon>mdi-delete</v-icon>
                </v-btn>
                <BackToTop/>
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
    import BackToTop from "@/components/BackToTop.vue";

    @Component({
        components: {NoteRender, Attachments, BackToTop},
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
            changed: new Date(),
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
            this.$store.commit("SET_PAGE_SUB_TITLE", value.title);
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