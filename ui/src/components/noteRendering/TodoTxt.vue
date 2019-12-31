<template>
    <v-card class="pa-2" tile>
        <v-list-item two-line v-for="(model, i) of models" :key="i" v-shortkey="['ctrl', 's']" @shortkey="editItem">
            <!-- View -->
            <v-list-item-avatar class="priority" :class="{ done: model.completed }" @click="editMode(i)"
                                v-if="indexEditing !== i">
                <span>{{model.priority}}</span>
            </v-list-item-avatar>
            <v-list-item-content :class="{ done: model.completed }" class="todo-item" @click="editMode(i)"
                                 v-if="indexEditing !== i">
                <v-list-item-title class="todo-description">{{model.description}}</v-list-item-title>
                <v-list-item-subtitle>
                    <span v-if="model.creationDate">created: {{model.creationDate | date}}</span>
                    <span v-if="model.completionDate">, completed: {{model.completionDate | date}}</span>
                </v-list-item-subtitle>
            </v-list-item-content>

            <!-- Edit -->
            <v-list-item-content v-if="indexEditing === i">
                <v-list-item-subtitle>
                    <textarea
                            v-model="model.fullText"
                            placeholder="Todo item..."
                    />
                </v-list-item-subtitle>
            </v-list-item-content>
            <v-list-item-action v-if="indexEditing === i">
                <v-btn title="Save" @click="editItem" text>
                    <v-icon>mdi-content-save</v-icon>
                </v-btn>
                <v-btn :title="model.completed ? 'Set to open' : 'Set to done'" @click="setCompletedStatus(model)" text>
                    <v-icon>mdi-check</v-icon>
                </v-btn>
                <v-btn title="Delete" @click="deleteItem" text>
                    <v-icon>mdi-delete</v-icon>
                </v-btn>
                <v-btn title="Cancel editing" @click="cancelEditing" text>
                    <v-icon>mdi-cancel</v-icon>
                </v-btn>
            </v-list-item-action>
        </v-list-item>
    </v-card>
</template>

<script lang="ts">
    import {Component, Prop, Vue} from "vue-property-decorator";
    import Note from "@/models/api/note";
    import {todoTxtToModels, todoTxtToString, singleTodoTxtToModel} from "@/services/todoTxtService";
    import {TodoTxtModel} from "@/models/todoTxtModel";
    import {UpdateNoteCommand} from "@/store/actions/notes";
    import {resources} from "@/resources";

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
        oldText: string = "";

        mounted() {
            this.models = todoTxtToModels(this.contents);
        }

        editItem() {
            let model = this.models[this.indexEditing];
            let newModel = singleTodoTxtToModel(model.fullText);
            this.models.splice(this.indexEditing, 1, newModel);
            this.save();
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

        deleteItem() {
            if(confirm(resources.areYouSureDeleteTodoItem)) {
                this.models.splice(this.indexEditing, 1);
                this.save();
            }
        }

        cancelEditing() {
            let model = this.models[this.indexEditing];
            model.fullText = this.oldText;
            let newModel = singleTodoTxtToModel(model.fullText);
            this.models.splice(this.indexEditing, 1, newModel);
            this.indexEditing = -1;
        }

        editMode(index: number) {
            let model = this.models[index];
            this.oldText = model.fullText;
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
    
    textarea {
        width: 100%;
    }
</style>