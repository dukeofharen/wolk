<template>
    <v-row>
        <v-col>
            <v-list-item
                    to="/noteForm"
                    v-if="isSignedIn && notebooks.length > 0"
            >
                <v-list-item-action>
                    <v-icon>mdi-plus</v-icon>
                </v-list-item-action>
                <v-list-item-title class="grey--text">Add note</v-list-item-title>
            </v-list-item>
            <v-list-item
                    to="/notebookForm"
                    v-if="isSignedIn"
            >
                <v-list-item-action>
                    <v-icon>mdi-plus</v-icon>
                </v-list-item-action>
                <v-list-item-title class="grey--text">Add notebook</v-list-item-title>
            </v-list-item>
            <v-list-item
                    @click="logOut"
                    v-if="isSignedIn"
            >
                <v-list-item-action>
                    <v-icon>mdi-exit-to-app</v-icon>
                </v-list-item-action>
                <v-list-item-title class="grey--text">Log out</v-list-item-title>
            </v-list-item>
            <v-divider
                    dark
                    class="my-4"
                    v-if="isSignedIn"
            ></v-divider>
            <div v-if="isSignedIn">
                <v-list-item
                        to="/overview"
                        v-if="isSignedIn"
                >
                    <v-list-item-action>
                        <v-icon>mdi-view-list</v-icon>
                    </v-list-item-action>
                    <v-list-item-title class="grey--text">Note overview</v-list-item-title>
                </v-list-item>
                <v-list-item
                        :to="{ name: 'notesList', params: {id: notebook.id}}"
                        v-for="notebook of notebooks"
                        :key="notebook.id"
                >
                    <v-list-item-action>
                        <v-icon>mdi-notebook</v-icon>
                    </v-list-item-action>
                    <v-list-item-title class="grey--text">{{notebook.name}}</v-list-item-title>
                </v-list-item>
            </div>
        </v-col>
    </v-row>
</template>

<script lang="ts">
    import {mapState} from "vuex";
    import {Component, Vue, Watch} from "vue-property-decorator";
    import Notebook from "../models/api/notebook";

    @Component({
        components: {},
        computed: mapState(["notebooks", "signedInUser"])
    })
    export default class NotebookBar extends Vue {
        constructor() {
            super();
        }

        mounted() {
            this.loadNotebooks();
        }

        get isSignedIn(): boolean {
            return this.$store.getters.isSignedIn;
        }

        @Watch("isSignedIn")
        loginChanged() {
            this.loadNotebooks();
        }

        logOut() {
            this.$store.commit("UNSET_SIGNED_IN_USER");
            this.$router.push({name: "login"});
        }

        private loadNotebooks() {
            this.$store.dispatch("loadNotebooks");
        }
    }
</script>

<style scoped>
</style>