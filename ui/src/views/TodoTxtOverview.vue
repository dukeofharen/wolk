import {NoteType} from "@/models/api/enums/noteType";
import {DueStatusType} from "@/services/todoTxtService";
import {DueStatusType} from "@/services/todoTxtService";
<template>
    <v-card class="pa-2" tile>
        <v-card-actions>
            <v-row no-gutters>
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
                @click="goToNote(model.noteId)"
                :style="{backgroundColor: getDueStatusColor(model)}">
            <v-list-item-avatar
                    class="priority"
                    :class="{ done: model.completed }">
                <span>{{model.priority}}</span>
            </v-list-item-avatar>
            <v-list-item-content
                    :class="{ done: model.completed }"
                    class="todo-item">
                <v-list-item-title class="todo-description">{{model.description}}</v-list-item-title>
                <v-list-item-subtitle>
                    <span v-if="model.creationDate">created: {{model.creationDate | date}}</span>
                    <span v-if="model.completionDate">, completed: {{model.completionDate | date}}</span>
                    <span v-if="model.dueDate">, due: {{model.dueDate | date}}</span>
                </v-list-item-subtitle>
            </v-list-item-content>
        </v-list-item>
    </v-card>
</template>

<script lang="ts">
    import {Component, Vue} from "vue-property-decorator";
    import Note from "@/models/api/note";
    import {
        DueStatusType,
        extractContextTags,
        extractProjectTags,
        todoTxtNotesToModels
    } from "@/services/todoTxtService";
    import {TodoTxtModel} from "@/models/todoTxtModel";
    import {mapState} from "vuex";
    import {LoadNotesQueryModel} from "@/models/store/loadNotesQueryModel";
    import {NoteType} from "@/models/api/enums/noteType";

    @Component({
        components: {},
        computed: mapState(["notes"])
    })
    export default class TodoTxtOverview extends Vue {
        notes!: Note[];
        projectTagFilter: string = "";
        contextTagFilter: string = "";

        mounted() {
            let query: LoadNotesQueryModel = {
                noteType: NoteType.TodoTxt,
                includeFullContents: true
            };
            this.$store.dispatch("loadNotes", query);
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
        
        goToNote(noteId: number) {
            this.$router.push({
                name: "viewNote",
                params: <any>{id: noteId}
            });
        }

        get projectTags() {
            return extractProjectTags(this.models);
        }

        get contextTags() {
            return extractContextTags(this.models);
        }

        get models() {
            return todoTxtNotesToModels(this.notes);
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