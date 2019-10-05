<template>
  <v-row>
    <v-col>
      <v-list-item
        @click="notebookClick(notebook)"
        v-for="notebook of notebooks"
        :key="notebook.id"
      >
        <v-list-item-action>
          <v-icon>mdi-notebook</v-icon>
        </v-list-item-action>
        <v-list-item-title class="grey--text">{{notebook.name}}</v-list-item-title>
      </v-list-item>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { mapState } from "vuex";
import { Component, Vue, Watch } from "vue-property-decorator";
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

  @Watch("signedInUser")
  loginChanged() {
    this.loadNotebooks();
  }

  notebookClick(notebook: Notebook) {
    this.$router.push({ name: "notesList", params: <any>{ id: notebook.id } });
  }

  private loadNotebooks() {
    this.$store.dispatch("loadNotebooks");
  }
}
</script>

<style scoped>
</style>