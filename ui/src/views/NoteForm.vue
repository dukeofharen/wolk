<template>
    <v-row
            v-shortkey="['ctrl', 's']"
            @shortkey="saveNote"
    >
        <v-col>
            <h1>{{noteId ? "Update" : "Create"}} note</h1>
            <v-dialog scrollable v-model="uiState.attachmentDialogOpened" v-if="noteId">
                <Attachments :noteId="note.id"/>
            </v-dialog>

            <div v-if="!previewing">
                <v-text-field
                        label="Note title"
                        type="text"
                        v-model="note.title"
                        @keyup="onChange"
                        @focus="showMetaInputs"
                />
                <transition name="fade">
                    <v-select
                            :items="notebooks"
                            placeholder="Select notebook..."
                            v-model="note.notebookId"
                            item-text="name"
                            item-value="id"
                            clearable
                            @change="onChange"
                            v-if="showMeta"
                    />
                </transition>
                <transition name="fade">
                    <v-select
                            :items="noteTypeNames"
                            placeholder="Select note type..."
                            v-model="note.noteType"
                            item-text="value"
                            item-value="key"
                            clearable
                            @change="onChange"
                            v-if="showMeta"
                    />
                </transition>
                <v-textarea
                        label="Note contents"
                        v-model="note.content"
                        auto-grow
                        @change="onChange"
                        @focus="hideMetaInputs"
                />
            </div>
            <NoteRender
                    v-if="previewing"
                    :contents="note.content"
                    :noteType="note.noteType"
            />
            <v-bottom-navigation color="indigo" fixed>
                <v-btn title="Save note" @click="saveNote">
                    <v-icon>mdi-content-save</v-icon>
                </v-btn>
                <v-btn title="Attachments" @click="showAttachments" v-if="noteId">
                    <v-icon>mdi-paperclip</v-icon>
                </v-btn>
                <v-btn title="View note" @click="viewNote" v-if="noteId">
                    <v-icon>mdi-eye</v-icon>
                </v-btn>
                <v-btn title="Preview note" @click="previewing = !previewing">
                    <v-icon>{{!previewing ? "mdi-eye-outline" : "mdi-arrow-left"}}</v-icon>
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
    import Notebook from "../models/api/notebook";
    import {getNoteTypeArray, NoteType} from "@/models/api/enums/noteType";
    import {KeyValuePair} from "@/models/keyValuePair";
    import {resources} from "@/resources";
    import NoteRender from "@/components/NoteRender.vue";
    import Attachments from "@/components/Attachments.vue";
    import {UiStateModel} from "@/models/store/uiStateModel";
    import BackToTop from "@/components/BackToTop.vue";

    @Component({
        components: {NoteRender, Attachments, BackToTop},
        computed: mapState(["notebooks", "currentNote", "uiState"]),
        beforeRouteLeave(to, from, next) {
            let form: NoteForm = this as NoteForm;
            if (!form.formDirty || confirm(resources.unsavedChanges)) {
                next();
            }
        }
    })
    export default class NoteForm extends Vue {
        noteId: string = "";
        noteTypeNames: Array<KeyValuePair<NoteType, string>> = getNoteTypeArray();
        notebooks!: Notebook[];
        note: Note = this.emptyNote();
        formDirty: boolean = false;
        previewing: boolean = false;
        showMeta: boolean = true;
        uiState!: UiStateModel;

        constructor() {
            super();
        }

        @Watch("$route")
        onRouteChanged() {
            this.reloadData();
            this.showMetaInputs()
        }

        @Watch("currentNote")
        onCurrentNoteChanged(value: Note) {
            this.note = value;
            this.$store.commit("SET_PAGE_SUB_TITLE", value.title);
        }

        mounted() {
            this.reloadData();
        }

        onChange(e: any) {
            this.formDirty = true;
            this.$store.commit("SET_PAGE_SUB_TITLE", this.note.title);
        }

        hideMetaInputs() {
            this.showMeta = false;
        }

        showMetaInputs() {
            this.showMeta = true;
        }

        saveNote() {
            if (!this.noteId) {
                this.$store.dispatch("createNote", this.note);
            } else {
                this.$store.dispatch("updateNote", {
                    note: this.note,
                    id: this.noteId
                });
            }

            this.formDirty = false;
        }

        viewNote() {
            this.$router.push({name: "viewNote", params: <any>{id: this.noteId}});
        }

        private reloadData() {
            this.noteId = <string>this.$route.params.id;
            if (!this.noteId) {
                this.note = this.emptyNote();
            } else {
                this.$store.dispatch("loadNote", this.noteId);
            }

            this.previewing = false;
            this.formDirty = false;
            let notebookId = <string>this.$route.query.notebookId;
            if (notebookId) {
                this.note.notebookId = parseInt(notebookId);
            }
        }

        emptyNote(): Note {
            return {
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
        }

        showAttachments() {
            this.uiState.attachmentDialogOpened = !this.uiState.attachmentDialogOpened;
        }
    }
</script>