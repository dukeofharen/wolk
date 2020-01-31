<template>
    <v-card class="pa-2" tile>
        <v-card-actions>
            <v-row no-gutters>
                <v-col cols="12">
                    <v-row>
                        <v-btn title="Add todo item" @click="addTodoItem" text>
                            <v-icon>mdi-plus</v-icon>
                        </v-btn>
                    </v-row>
                </v-col>
                <v-col cols="12">
                    <TodoTxtFilter :models="models"/>
                </v-col>
            </v-row>
        </v-card-actions>
        <v-list-item
                two-line
                v-for="(model, i) of filteredModels"
                :key="i" v-shortkey="['ctrl', 's']"
                @shortkey="editItem"
                :style="{backgroundColor: getDueStatusColor(model)}">
            <!-- View -->
            <v-list-item-avatar
                    class="priority"
                    :class="{ done: model.completed }"
                    @click="editMode(i)"
                    v-if="indexEditing !== i">
                <span>{{model.priority}}</span>
            </v-list-item-avatar>
            <v-list-item-content
                    :class="{ done: model.completed }"
                    class="todo-item"
                    @click="editMode(i)"
                    v-if="indexEditing !== i">
                <v-list-item-title class="todo-description" v-html="parseMarkdown(model.description)"/>
                <v-list-item-subtitle>
                    <span v-if="model.creationDate">created: {{model.creationDate | date}}</span>
                    <span v-if="model.completionDate">, completed: {{model.completionDate | date}}</span>
                    <span v-if="model.dueDate">, due: {{model.dueDate | date}}</span>
                </v-list-item-subtitle>
            </v-list-item-content>

            <!-- Edit -->
            <v-list-item-action class="edit-buttons" v-if="indexEditing === i">
                <v-row no-gutters>
                    <v-col cols="6">
                        <v-btn title="Save" @click="editItem" text>
                            <v-icon>mdi-content-save</v-icon>
                        </v-btn>
                    </v-col>
                    <v-col cols="6">
                        <v-btn :title="model.completed ? 'Set to open' : 'Set to done'"
                               @click="setCompletedStatus(model)" text>
                            <v-icon>mdi-check</v-icon>
                        </v-btn>
                    </v-col>
                </v-row>
                <v-row no-gutters>
                    <v-col cols="6">
                        <v-btn title="Delete" @click="deleteItem" text>
                            <v-icon>mdi-delete</v-icon>
                        </v-btn>
                    </v-col>
                    <v-col cols="6">
                        <v-btn title="Cancel editing" @click="cancelEditing" text>
                            <v-icon>mdi-cancel</v-icon>
                        </v-btn>
                    </v-col>
                </v-row>
                <v-row no-gutters>
                    <v-col cols="6">
                        <v-btn title="Show form" @click="showOrHideForm" text>
                            <v-icon>mdi-card-text-outline</v-icon>
                        </v-btn>
                    </v-col>
                </v-row>
            </v-list-item-action>
            <template v-if="indexEditing === i">
                <v-list-item-content v-if="!showForm">
                    <textarea
                            v-model="model.fullText"
                            placeholder="Todo item..."
                    />
                </v-list-item-content>
                <v-list-item-content v-if="showForm">
                    <v-row no-gutters>
                        <v-col cols="12">
                            <v-select
                                    :items="priorities"
                                    label="Priority..."
                                    v-model="model.priority"
                                    clearable
                            />
                        </v-col>
                    </v-row>
                    <v-row no-gutters>
                        <v-col cols="12">
                            <v-textarea
                                    label="Description..."
                                    type="text"
                                    v-model="model.description"
                            />
                        </v-col>
                    </v-row>
                </v-list-item-content>
            </template>
        </v-list-item>
    </v-card>
</template>

<script lang="ts">
    import {Component, Prop, Vue} from "vue-property-decorator";
    import Note from "@/models/api/note";
    import {
        singleTodoTxtToModel, singleTodoTxtToString,
        todoTxtToModels,
        todoTxtToString
    } from "@/services/todoTxtService";
    import {TodoTxtModel} from "@/models/todoTxtModel";
    import {UpdateNoteCommand} from "@/store/actions/notes";
    import {resources} from "@/resources";
    import moment from "moment";
    import TodoTxtFilter from "@/components/TodoTxtFilter.vue";
    import {
        filterTodoItems,
        getDueStatusColor,
        parseMarkdown,
        getPriorities
    } from "@/utilities/todoTxtUiHelper";

    @Component({
        components: {TodoTxtFilter}
    })
    export default class TodoTxt extends Vue {
        @Prop()
        contents!: string;

        @Prop()
        note!: Note;

        priorities = getPriorities();
        models: TodoTxtModel[] = [];
        indexEditing: number = -1;
        oldText: string = "";
        showForm: boolean = false;

        mounted() {
            this.models = todoTxtToModels(this.contents, undefined);

            // If hashCode is set, find the specific todoitem and open it
            let hashCodeText = <string>this.$route.query.hashCode;
            if (hashCodeText) {
                let hashCode = parseInt(hashCodeText);
                let item = this.models.find(m => m.hashCode === hashCode);
                if (item) {
                    this.editMode(this.models.indexOf(item));
                }
            }
        }

        editItem() {
            let model = this.models[this.indexEditing];
            if (!this.showForm) {
                // The plain text input is used; create a model based on the full text. 
                let newModel = singleTodoTxtToModel(model.fullText);
                this.models.splice(this.indexEditing, 1, newModel);
            } else {
                // The form is used; update the fullText based on the model.
                model.fullText = singleTodoTxtToString(model);
            }

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
            if (confirm(resources.areYouSureDeleteTodoItem)) {
                this.models.splice(this.indexEditing, 1);
                this.save();
            }
        }

        addTodoItem() {
            let now = moment(new Date()).format('YYYY-MM-DD');
            let model: TodoTxtModel = {
                fullText: `(A) ${now} note-description +project-tag @context-tag`,
                completed: false,
                creationDate: undefined,
                completionDate: undefined,
                description: "",
                priority: "",
                contextTags: [],
                projectTags: [],
                hashCode: 0
            };
            this.models.unshift(model);
            this.indexEditing = 0;
        }

        cancelEditing() {
            if (this.oldText) {
                let model = this.models[this.indexEditing];
                model.fullText = this.oldText;
                let newModel = singleTodoTxtToModel(model.fullText);
                this.models.splice(this.indexEditing, 1, newModel);
            }

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

        getDueStatusColor(model: TodoTxtModel) {
            return getDueStatusColor(model);
        }

        parseMarkdown(input: string) {
            return parseMarkdown(input);
        }

        showOrHideForm() {
            this.showForm = !this.showForm;
        }

        get todoTxtFilter() {
            return this.$store.getters.todoTxtFilter
        }

        get filteredModels() {
            return filterTodoItems(this.models, this.todoTxtFilter);
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
        height: 100px;
    }

    .edit-buttons {
        margin-right: 0 !important;
        width: 128px;
    }

    .edit-buttons .row {
        width: 100%;
    }
</style>