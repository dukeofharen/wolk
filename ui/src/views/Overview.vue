import {firstBy} from "thenby";
<template>
    <v-row>
        <v-col>
            <h1>Overview</h1>
            <OverviewNote
                    v-for="note of filteredNotes"
                    :key="note.id"
                    v-bind:note="note"
            />
        </v-col>
        <v-bottom-navigation color="indigo" fixed>
            <BackToTop/>
        </v-bottom-navigation>
    </v-row>
</template>

<script lang="ts">
    import {mapState} from "vuex";
    import {Component, Vue, Watch} from "vue-property-decorator";
    import OverviewNote from "@/components/OverviewNote.vue";
    import {LoadNotesQueryModel} from "@/models/store/loadNotesQueryModel";
    import BackToTop from "@/components/BackToTop.vue";
    import Note from "@/models/api/note";
    import {firstBy} from "thenby";

    @Component({
        components: {OverviewNote, BackToTop},
        computed: mapState(["notes"])
    })
    export default class Overview extends Vue {
        notes!: Note[];
        
        constructor() {
            super();
        }

        mounted() {
            this.loadNotes();
            this.$store.commit("SET_PAGE_SUB_TITLE", "Notes overview");
        }

        @Watch("$route")
        onRouteChanged() {
            this.loadNotes();
        }

        get filteredNotes(): Note[] {
            let filteredNotes = this.notes;
            filteredNotes.sort(firstBy("opened", -1));
            return filteredNotes;
        }

        private loadNotes() {
            let query: LoadNotesQueryModel = {
                searchTerm: <string>this.$route.query.searchTerm
            };
            this.$store.dispatch("loadNotes", query);
        }
    }
</script>

<style scoped>
</style>