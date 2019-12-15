<template>
    <v-row>
        <v-col>
            <v-card>
                <v-card-title>
                    <v-btn text :to="{ name: 'viewNote', params: {id: note.id}}">
                        <v-icon>mdi-eye</v-icon>
                    </v-btn>
                    <v-btn text :to="{ name: 'noteForm', params: {id: note.id}}">
                        <v-icon>mdi-lead-pencil</v-icon>
                    </v-btn>
                  <span>{{note.title}}</span>
                </v-card-title>
                <v-card-text>
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
                        <v-chip title="Note type">
                            <v-icon left>mdi-book-variant</v-icon>
                            {{noteTypeName}}
                        </v-chip>
                    </div>
                    <div>
                        {{note.preview}}
                    </div>
                </v-card-text>
            </v-card>
        </v-col>
    </v-row>
</template>

<script lang="ts">
    import {Component, Vue, Watch, Prop} from "vue-property-decorator";
    import Notebook from "../models/api/notebook";
    import Note from "../models/api/note";
    import {NoteTypeMap} from "../models/api/enums/noteType";
    import {resources} from "@/resources";

    @Component({
        components: {}
    })
    export default class OverviewNote extends Vue {
        @Prop()
        note!: Note;
        noteTypeName: string = "";

        constructor() {
            super();
            this.noteTypeName = <string>NoteTypeMap.get(this.note.noteType);
        }
    }
</script>

<style scoped>
    .v-card .v-btn {
        width: 40px;
        min-width: 0;
    }
</style>