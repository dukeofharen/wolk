<template>
    <v-container class="lighten-5">
        <v-row no-gutters>
            <v-col cols="12" sm="4" v-for="(model, i) of models" :key="i">
                <v-card class="pa-2 sticky-note" tile>
                    <v-card-title class="sticky-title">
                        <div v-if="indexEditing !== i" @click="edit(i)">{{model.title}}</div>
                        <v-text-field
                                label="Note title"
                                type="text"
                                v-model="model.title"
                                v-if="indexEditing === i"
                        />
                    </v-card-title>
                    <v-card-text class="sticky-contents">
                        <div v-if="indexEditing !== i" v-html="marked(model.contents)" @click="edit(i)" />
                        <textarea
                                v-model="model.contents"
                                v-if="indexEditing === i"
                                placeholder="Note contents"
                        />
                    </v-card-text>
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
        
        data() {
            return{
                marked: marked
            }
        }

        mounted() {
            this.models = stickyNotesToModel(this.contents);
        }
        
        edit(index: number) {
            if(this.indexEditing != -1) {
                this.saveNote();
            }
            
            this.indexEditing = index;
        }
        
        saveNote() {
            if(this.note) {
                this.note.content = stickyNotesToString(this.models);
                let command: UpdateNoteCommand = {
                    id: this.note.id,
                    note: this.note
                };
                this.$store.dispatch("updateNote", command);
            }
        }
    }
</script>

<style scoped>
    .sticky-note {
        margin: 10px;
    }
    
    .sticky-title, .sticky-contents{
        cursor: pointer;
    }
    
    .sticky-contents textarea {
        width: 100%;
        min-height: 200px;
    }
</style>