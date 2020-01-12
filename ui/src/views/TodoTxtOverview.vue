<template>
    <v-row>
        <v-col>
            <h1>All todo items</h1>
            <v-card class="pa-2" tile>
                <v-card-actions>
                    <v-row no-gutters>
                        <v-col cols="12">
                            <TodoTxtFilter :models="models" />
                        </v-col>
                    </v-row>
                </v-card-actions>
                <v-list-item
                        two-line
                        v-for="(model, i) of filteredModels"
                        :key="i"
                        @click="goToNote(model.noteId, model.hashCode)"
                        :style="{backgroundColor: getDueStatusColor(model)}">
                    <v-list-item-avatar
                            class="priority"
                            :class="{ done: model.completed }">
                        <span>{{model.priority}}</span>
                    </v-list-item-avatar>
                    <v-list-item-content
                            :class="{ done: model.completed }"
                            class="todo-item">
                        <v-list-item-title class="todo-description" v-html="parseMarkdown(model.description)" />
                        <v-list-item-subtitle>
                            <span v-if="model.creationDate">created: {{model.creationDate | date}}</span>
                            <span v-if="model.completionDate">, completed: {{model.completionDate | date}}</span>
                            <span v-if="model.dueDate">, due: {{model.dueDate | date}}</span>
                        </v-list-item-subtitle>
                    </v-list-item-content>
                </v-list-item>
            </v-card>
        </v-col>
    </v-row>
</template>

<script lang="ts">
    import {Component, Vue} from "vue-property-decorator";
    import Note from "@/models/api/note";
    import {
        todoTxtNotesToModels
    } from "@/services/todoTxtService";
    import {TodoTxtModel} from "@/models/todoTxtModel";
    import {mapState} from "vuex";
    import {LoadNotesQueryModel} from "@/models/store/loadNotesQueryModel";
    import {NoteType} from "@/models/api/enums/noteType";
    import marked from "marked";
    import TodoTxtFilter from "@/components/TodoTxtFilter.vue";
    import {
        filterTodoItems,
        getDueStatusColor,
        parseMarkdown} from "@/utilities/todoTxtUiHelper";

    @Component({
        components: {TodoTxtFilter},
        computed: mapState(["notes"])
    })
    export default class TodoTxtOverview extends Vue {
        notes!: Note[];

        mounted() {
            let query: LoadNotesQueryModel = {
                noteType: NoteType.TodoTxt,
                includeFullContents: true
            };
            this.$store.dispatch("loadNotes", query);
            this.$store.commit("SET_PAGE_SUB_TITLE", "All todo items");
        }

        getDueStatusColor(model: TodoTxtModel) {
            return getDueStatusColor(model);
        }

        goToNote(noteId: number, hashCode: number) {
            this.$router.push({
                name: "viewNote",
                params: <any>{id: noteId},
                query: <any>{hashCode: hashCode}
            });
        }

        parseMarkdown(input: string) {
            return parseMarkdown(input);
        }

        get models() {
            return todoTxtNotesToModels(this.notes);
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
</style>