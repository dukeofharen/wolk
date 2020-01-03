<template>
    <v-row>
        <v-col class="notebook-bar">
            <transition name="fade">
                <v-list-item
                        :to="{ name: 'noteForm'}"
                        v-if="isSignedIn && notebooks.length > 0"
                >
                    <v-list-item-action>
                        <v-icon>mdi-plus</v-icon>
                    </v-list-item-action>
                    <v-list-item-title class="grey--text">Add note</v-list-item-title>
                </v-list-item>
            </transition>
            <transition name="fade">
                <v-list-item
                        :to="{ name: 'notebookForm'}"
                        v-if="isSignedIn"
                >
                    <v-list-item-action>
                        <v-icon>mdi-plus</v-icon>
                    </v-list-item-action>
                    <v-list-item-title class="grey--text">Add notebook</v-list-item-title>
                </v-list-item>
            </transition>
            <transition name="fade">
                <v-list-item
                        :to="{ name: 'settings'}"
                        v-if="isSignedIn"
                >
                    <v-list-item-action>
                        <v-icon>mdi-cogs</v-icon>
                    </v-list-item-action>
                    <v-list-item-title class="grey--text">Settings</v-list-item-title>
                </v-list-item>
            </transition>
            <transition name="fade">
                <v-list-item
                        @click="logOut"
                        v-if="isSignedIn"
                >
                    <v-list-item-action>
                        <v-icon>mdi-exit-to-app</v-icon>
                    </v-list-item-action>
                    <v-list-item-title class="grey--text">Log out</v-list-item-title>
                </v-list-item>
            </transition>
            <transition name="fade">
                <v-divider
                        dark
                        class="my-4"
                        v-if="isSignedIn"
                />
            </transition>
            <transition name="fade">
                <div v-if="isSignedIn">
                    <v-list-item
                            :to="{ name: 'overview'}"
                            v-if="isSignedIn"
                    >
                        <v-list-item-action>
                            <v-icon>mdi-view-list</v-icon>
                        </v-list-item-action>
                        <v-list-item-title class="grey--text">Note overview</v-list-item-title>
                    </v-list-item>
                    <v-list-item
                            :to="{ name: 'todoTxtOverview'}"
                            v-if="isSignedIn"
                    >
                        <v-list-item-action>
                            <v-icon>mdi-check-all</v-icon>
                        </v-list-item-action>
                        <v-list-item-title class="grey--text">Todo item overview</v-list-item-title>
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
            </transition>
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
    .notebook-bar {
        margin-bottom: 50px;
    }
</style>