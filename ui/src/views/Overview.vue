<template>
    <v-row>
        <v-col>
            <h1>Overview</h1>
            <OverviewNote
                    v-for="note of notes"
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

    @Component({
        components: {OverviewNote, BackToTop},
        computed: mapState(["notes"])
    })
    export default class Overview extends Vue {
        constructor() {
            super();
        }

        mounted() {
            this.loadNotes();
        }

        @Watch("$route")
        onRouteChanged() {
            this.loadNotes();
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