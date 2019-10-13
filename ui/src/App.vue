<template>
  <v-app id="keep">
    <v-app-bar
      app
      clipped-left
      color="amber"
    >
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
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
      ></v-text-field>
      <div class="flex-grow-1"></div>
    </v-app-bar>

    <v-navigation-drawer
      v-model="drawer"
      app
      clipped
      color="grey lighten-4"
    >
      <v-list-item
        @click="addNotebook"
        v-if="isSignedIn"
      >
        <v-list-item-action>
          <v-icon>mdi-plus</v-icon>
        </v-list-item-action>
        <v-list-item-title class="grey--text">Add notebook</v-list-item-title>
      </v-list-item>
      <v-divider
        dark
        class="my-4"
        v-if="isSignedIn"
      ></v-divider>
      <NotebookBar></NotebookBar>
    </v-navigation-drawer>

    <v-content>
      <v-container
        fluid
        class="lighten-4"
      >
        <router-view></router-view>
      </v-container>
    </v-content>
    <ToastMessages></ToastMessages>
  </v-app>
</template>

<script lang="ts">
import { mapState } from "vuex";
import { Component, Vue } from "vue-property-decorator";
import NotebookBar from "@/components/NotebookBar.vue";
import ToastMessages from "@/components/ToastMessages.vue";

@Component({
  components: { NotebookBar, ToastMessages },
  computed: mapState(["notebooks"])
})
export default class App extends Vue {
  drawer: boolean = true;

  get isSignedIn(): boolean {
    return this.$store.getters.isSignedIn;
  }

  addNotebook() {
    this.$router.push({ name: "addNotebook" });
  }
}
</script>
