<template>
    <v-container class="lighten-5">
        <v-row no-gutters>
            <v-col cols="12" sm="12">
                <v-btn title="Add note" @click="addNote(0)" text>
                    <v-icon>mdi-plus</v-icon>
                </v-btn>
            </v-col>
            <v-col cols="12" sm="4" v-for="(model, i) of models" :key="i">
                <v-card class="pa-2 sticky-note" tile>
                    <v-card-title class="sticky-title">
                        <div v-if="indexEditing !== i" @click="edit(i)">{{model.title}}</div>
                        <div v-if="indexEditing === i">
                            <v-text-field
                                    label="Note title"
                                    type="text"
                                    v-model="model.title"
                            />
                        </div>
                    </v-card-title>
                    <v-card-text class="sticky-contents">
                        <div v-if="indexEditing !== i" v-html="marked(model.contents)" @click="edit(i)"/>
                        <textarea
                                v-model="model.contents"
                                v-if="indexEditing === i"
                                placeholder="Note contents"
                        />
                    </v-card-text>
                    <v-card-actions>
                        <v-btn title="Save note" @click="saveNote" v-if="indexEditing === i" text>
                            <v-icon>mdi-content-save</v-icon>
                        </v-btn>
                        <v-btn title="Cancel editing" @click="cancelEditing" v-if="indexEditing === i" text>
                            <v-icon>mdi-cancel</v-icon>
                        </v-btn>
                        <v-btn title="Delete note" @click="deleteNote(i)" v-if="indexEditing !== i" text>
                            <v-icon>mdi-delete</v-icon>
                        </v-btn>
                        <v-btn title="Move one lower" @click="moveOneLower(i)" v-if="indexEditing !== i" text>
                            <v-icon>mdi-arrow-down-bold</v-icon>
                        </v-btn>
                        <v-btn title="Move one higher" @click="moveOneHigher(i)" v-if="indexEditing !== i" text>
                            <v-icon>mdi-arrow-up-bold</v-icon>
                        </v-btn>
                        <v-btn title="Add before" @click="addNote(i)" v-if="indexEditing !== i" text>
                            <v-icon>mdi-table-column-plus-before</v-icon>
                        </v-btn>
                        <v-btn title="Add after" @click="addNote(i+1)" v-if="indexEditing !== i" text>
                            <v-icon>mdi-table-column-plus-after</v-icon>
                        </v-btn>
                    </v-card-actions>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>

<script lang="ts">
    import {Component, Prop, Vue} from "vue-property-decorator";
    import {stickyNotesToModel, stickyNotesToString} from "@/services/stickyNoteService";
    import {StickyNotesModel} from "@/models/stickyNotesModel";
    import marked from "marked";
    import {UpdateNoteCommand} from "@/store/actions/notes";
    import Note from "@/models/api/note";
    import {resources} from "@/resources";
    import arrayMove from 'array-move';

    @Component({
        components: {}
    })
    export default class StickyNotes extends Vue {
        @Prop()
        contents!: string;

        @Prop()
        note!: Note;

        models: StickyNotesModel[] = [];
        indexEditing: number = -1;

        oldTitle = "";
        oldContents = "";

        data() {
            return {
                marked: marked
            }
        }

        mounted() {
            this.models = stickyNotesToModel(this.contents);
        }

        edit(index: number) {
            if (this.indexEditing != -1) {
                this.saveNote();
            }

            let model = this.models[index];
            this.oldTitle = model.title;
            this.oldContents = model.contents;
            this.indexEditing = index;
        }

        cancelEditing() {
            let model = this.models[this.indexEditing];
            model.title = this.oldTitle;
            model.contents = this.oldContents;
            this.indexEditing = -1;
        }

        addNote(index: number) {
            this.models.splice(index, 0, {
                contents: "",
                title: ""
            });
            this.edit(index);
        }

        saveNote() {
            if (this.indexEditing > -1 && this.models.length > this.indexEditing) {
                let model = this.models[this.indexEditing];
                this.oldTitle = model.title;
                this.oldContents = model.contents;
            }

            if (this.note) {
                this.note.content = stickyNotesToString(this.models);
                let command: UpdateNoteCommand = {
                    id: this.note.id,
                    note: this.note
                };
                this.$store.dispatch("updateNote", command);
            }
        }

        deleteNote(index: number) {
            if (confirm(resources.areYouSureDeleteStickyNote)) {
                this.models.splice(index, 1);
                this.saveNote();
            }
        }

        moveOneLower(index: number) {
            if (index < this.models.length - 1) {
                this.models = arrayMove(this.models, index, index + 1);
                this.saveNote();
            }
        }

        moveOneHigher(index: number) {
            if (index > 0) {
                this.models = arrayMove(this.models, index, index - 1);
                this.saveNote();
            }
        }
    }
</script>

<style scoped>
    .sticky-note {
        margin: 10px;
    }

    .sticky-title, .sticky-contents {
        cursor: pointer;
    }

    .sticky-title > div {
        width: 100%;
    }

    .sticky-contents textarea {
        width: 100%;
        min-height: 200px;
    }

    .sticky-note .v-btn {
        min-width: 0;
        width: 35px;
    }
</style>