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
                    <v-row>
                        <v-select
                                :items="projectTags"
                                placeholder="Filter on project tag..."
                                v-model="projectTagFilter"
                                clearable
                        />
                    </v-row>
                </v-col>
                <v-col cols="12">
                    <v-row>
                        <v-select
                                :items="contextTags"
                                placeholder="Filter on context tag..."
                                v-model="contextTagFilter"
                                clearable
                        />
                    </v-row>
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
                <v-list-item-title class="todo-description">{{model.description}}</v-list-item-title>
                <v-list-item-subtitle>
                    <span v-if="model.creationDate">created: {{model.creationDate | date}}</span>
                    <span v-if="model.completionDate">, completed: {{model.completionDate | date}}</span>
                    <span v-if="model.dueDate">, due: {{model.dueDate | date}}</span>
                </v-list-item-subtitle>
            </v-list-item-content>

            <!-- Edit -->
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
            <v-list-item-content v-if="indexEditing === i">
                <v-list-item-subtitle>
                    <textarea
                            v-model="model.fullText"
                            placeholder="Todo item..."
                    />
                </v-list-item-subtitle>
            </v-list-item-content>
        </v-list-item>
    </v-card>
</template>

<script lang="ts">
    import {Component, Prop, Vue} from "vue-property-decorator";
    import Note from "@/models/api/note";
    import {
        DueStatusType,
        extractContextTags,
        extractProjectTags,
        singleTodoTxtToModel,
        todoTxtToModels,
        todoTxtToString
    } from "@/services/todoTxtService";
    import {TodoTxtModel} from "@/models/todoTxtModel";
    import {UpdateNoteCommand} from "@/store/actions/notes";
    import {resources} from "@/resources";
    import moment from "moment";

    @Component({
        components: {}
    })
    export default class TodoTxt extends Vue {
        @Prop()
        contents!: string;

        @Prop()
        note!: Note;

        models: TodoTxtModel[] = [];
        indexEditing: number = -1;
        oldText: string = "";
        projectTagFilter: string = "";
        contextTagFilter: string = "";

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
            let defaultColor = "#ffffff";
            if (model.completed) {
                return defaultColor;
            }

            switch (model.dueStatus) {
                case DueStatusType.OVERDUE:
                    return "#ff8f8f";
                case DueStatusType.DUE_TODAY:
                    return "#ffcf8f";
                case DueStatusType.DUE_IN_A_DAY:
                    return "#faff8f";
                default:
                    return defaultColor;
            }
        }

        get projectTags() {
            return extractProjectTags(this.models);
        }

        get contextTags() {
            return extractContextTags(this.models);
        }

        get filteredModels() {
            let result = this.models;
            if (this.projectTagFilter) {
                result = result.filter(r => r.projectTags.indexOf(this.projectTagFilter) > -1);
            }

            if (this.contextTagFilter) {
                result = result.filter(r => r.contextTags.indexOf(this.contextTagFilter) > -1);
            }

            return result;
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
</style>