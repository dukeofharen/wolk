<template>
    <v-app id="keep">
        <v-app-bar
                app
                clipped-left
                color="blue lighten-1"
        >
            <v-app-bar-nav-icon @click="drawer = !drawer"/>
            <span class="title ml-3 mr-5">
        Wolk&nbsp;
        <span class="font-weight-light">Notes</span>
      </span>
            <v-text-field
                    solo-inverted
                    flat
                    hide-details
                    label="Search"
                    prepend-inner-icon="mdi-magnify"
                    v-model="searchTerm"
                    @input="searchTermChanged"
                    v-if="isSignedIn"
            />
            <div class="flex-grow-1"></div>
        </v-app-bar>

        <v-navigation-drawer
                v-model="drawer"
                app
                clipped
                color="grey lighten-4"
        >
            <NotebookBar/>
        </v-navigation-drawer>

        <v-content>
            <v-container
                    fluid
                    class="lighten-4"
            >
                <router-view/>
            </v-container>
        </v-content>
        <ToastMessages/>
        <DocumentEvents/>
    </v-app>
</template>

<script lang="ts">
    import {mapState} from "vuex";
    import {Component, Vue} from "vue-property-decorator";
    import NotebookBar from "@/components/NotebookBar.vue";
    import ToastMessages from "@/components/ToastMessages.vue";
    import DocumentEvents from "@/components/DocumentEvents.vue";

    @Component({
        components: {NotebookBar, ToastMessages, DocumentEvents},
        computed: mapState(["notebooks"])
    })
    export default class App extends Vue {
        drawer: boolean = true;
        searchTerm: string = "";
        timeoutRef: number = 0;

        get isSignedIn(): boolean {
            return this.$store.getters.isSignedIn;
        }

        searchTermChanged() {
            if (this.timeoutRef) {
                clearTimeout(this.timeoutRef);
            }
            
            this.timeoutRef = setTimeout(() => {
                if (this.searchTerm) {
                    this.$router.push({
                        name: "overview",
                        query: <any>{searchTerm: this.searchTerm}
                    });
                } else {
                    this.$router.push({
                        name: "overview"
                    });
                }
            }, 250);
        }
    }
</script>
