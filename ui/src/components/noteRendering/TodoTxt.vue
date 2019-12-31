<template>
    <v-card class="pa-2" tile>
        <v-list-item two-line v-for="(model, i) of models" :key="i">
            <v-list-item-avatar class="priority">
                <span>{{model.priority}}</span>
            </v-list-item-avatar>
            <v-list-item-content>
                <v-list-item-title class="todo-description">{{model.description}}</v-list-item-title>
                <v-list-item-subtitle><span v-if="model.creationDate">created: {{model.creationDate | date}}</span><span v-if="model.completionDate">, completed: {{model.completionDate | date}}</span></v-list-item-subtitle>
            </v-list-item-content>
        </v-list-item>
    </v-card>
</template>

<script lang="ts">
    import {Component, Prop, Vue} from "vue-property-decorator";
    import Note from "@/models/api/note";
    import {todoTxtToModels} from "@/services/todoTxtService";
    import {TodoTxtModel} from "@/models/todoTxtModel";

    @Component({
        components: {}
    })
    export default class StickyNotes extends Vue {
        @Prop()
        contents!: string;

        @Prop()
        note!: Note;

        models: TodoTxtModel[] = [];
        indexEditing: number = -1;

        mounted() {
            this.models = todoTxtToModels(this.contents);
        }

        saveNote() {
        }
    }
</script>

<style scoped>
    .v-card .v-list-item {
        border-bottom: 1px solid #cecece;
    }

    .todo-description {
        white-space: normal;
    }
    
    .priority {
        height: 10px !important;
        width: 10px !important;
        min-width: 10px !important;
        margin-right: 16px !important;
    }
</style>