<template>
    <v-row>
        <v-col>
            <transition name="fade">
                <h1 v-if="currentNotebook.name">{{currentNotebook.name}}</h1>
            </transition>
            <v-row>
                <v-col>
                    <transition-group name="fade">
                        <OverviewNote
                                v-for="note of filteredNotes"
                                :key="note.id"
                                v-bind:note="note"
                        />
                    </transition-group>
                </v-col>
            </v-row>
        </v-col>
        <v-bottom-navigation color="indigo" fixed>
            <v-btn title="Add note" :to="{ name: 'noteForm', query: {notebookId: currentNotebook.id}}">
                <v-icon>mdi-plus</v-icon>
            </v-btn>
            <v-btn title="Update notebook" :to="{ name: 'notebookForm', params: {id: currentNotebook.id}}">
                <v-icon>mdi-lead-pencil</v-icon>
            </v-btn>
            <v-btn title="Delete notebook" @click="deleteNotebook" color="error">
                <v-icon>mdi-delete</v-icon>
            </v-btn>
            <BackToTop/>
        </v-bottom-navigation>
    </v-row>
</template>

<script lang="ts">
    import {mapState} from "vuex";
    import {Component, Vue, Watch} from "vue-property-decorator";
    import OverviewNote from "@/components/OverviewNote.vue";
    import Notebook from "../models/api/notebook";
    import {resources} from "@/resources";
    import {LoadNotesQueryModel} from "@/models/store/loadNotesQueryModel";
    import BackToTop from "@/components/BackToTop.vue";
    import Note from "@/models/api/note";
    import {firstBy} from "thenby";

    @Component({
        components: {OverviewNote, BackToTop},
        computed: mapState(["notes", "currentNotebook"])
    })
    export default class NotesList extends Vue {
        currentNotebook!: Notebook;
        notes!: Note[];

        constructor() {
            super();
        }

        beforeMount() {
            this.$store.commit("SET_CURRENT_NOTEBOOK", this.emptyNotebook());
            this.$store.commit("SET_NOTES", []);
        }

        mounted() {
            this.reloadData();
        }

        @Watch("$route")
        onRouteChanged() {
            this.reloadData();
        }

        @Watch("currentNotebook")
        onCurrentNotebookChanged(newValue: Notebook) {
            this.$store.commit("SET_PAGE_SUB_TITLE", newValue.name);
        }

        deleteNotebook() {
            if (confirm(resources.areYouSureDeleteNotebook)) {
                this.$store.dispatch("deleteNotebook", this.currentNotebook.id);
            }
        }

        emptyNotebook(): Notebook {
            return {
                id: 0,
                name: "",
                created: new Date(),
                updated: new Date()
            };
        }

        get filteredNotes(): Note[] {
            let filteredNotes = this.notes;
            filteredNotes.sort(firstBy("opened", -1));
            return filteredNotes;
        }

        private reloadData() {
            let query: LoadNotesQueryModel = {
                notebookId: parseInt(this.$route.params.id),
                searchTerm: undefined
            };
            this.$store.dispatch("loadNotes", query);
            this.$store.dispatch("loadNotebook", this.$route.params.id);
        }
    }
</script>

<style scoped>
</style>