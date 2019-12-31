<template>
    <v-card class="pa-2" tile>
        <v-list-item two-line v-for="(model, i) of models" :key="i">
            <!-- View -->
            <v-list-item-avatar class="priority" :class="{ done: model.completed }" @click="editItem(i)" v-if="indexEditing !== i">
                <span>{{model.priority}}</span>
            </v-list-item-avatar>
            <v-list-item-content :class="{ done: model.completed }" class="todo-item" @click="editItem(i)" v-if="indexEditing !== i">
                <v-list-item-title class="todo-description">{{model.description}}</v-list-item-title>
                <v-list-item-subtitle>
                    <span v-if="model.creationDate">created: {{model.creationDate | date}}</span>
                    <span v-if="model.completionDate">, completed: {{model.completionDate | date}}</span>
                </v-list-item-subtitle>
            </v-list-item-content>
            
            <!-- Edit -->
            <v-list-item-avatar class="done-button" v-if="indexEditing === i">
                <v-btn :title="model.completed ? 'Set to open' : 'Set to done'" @click="setCompletedStatus(model)" text>
                    <v-icon>mdi-check</v-icon>
                </v-btn>
            </v-list-item-avatar>
            <v-list-item-subtitle v-if="indexEditing === i">
                
            </v-list-item-subtitle>
        </v-list-item>
    </v-card>
</template>

<script lang="ts">
    import {Component, Prop, Vue} from "vue-property-decorator";
    import Note from "@/models/api/note";
    import {todoTxtToModels, todoTxtToString} from "@/services/todoTxtService";
    import {TodoTxtModel} from "@/models/todoTxtModel";
    import {stickyNotesToString} from "@/services/stickyNoteService";
    import {UpdateNoteCommand} from "@/store/actions/notes";

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

        save() {
            let todoTxtText = todoTxtToString(this.models);
            this.models = todoTxtToModels(todoTxtText);
            this.indexEditing = -1;
            if (this.note) {
                this.note.content = todoTxtText;
                let command: UpdateNoteCommand = {
                    id: this.note.id,
                    note: this.note
                };
                this.$store.dispatch("updateNote", command);
            }
        }
        
        editItem(index: number) {
            this.indexEditing = index;
        }
        
        setCompletedStatus(model: TodoTxtModel) {
            model.completionDate = model.completed ? undefined : new Date();
            model.completed = !model.completed;
            this.save();
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

    .priority, .done-button {
        height: 10px !important;
        width: 10px !important;
        min-width: 10px !important;
        margin-right: 16px !important;
    }
    
    .done {
        text-decoration: line-through;
        color: #aaaaaa !important;
    }
    
    .priority, .todo-item {
        cursor: pointer;
    }
</style>