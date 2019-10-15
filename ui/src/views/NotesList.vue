<template>
  <v-row>
    <v-col>
      <h1>{{currentNotebook.name}}</h1>
      <v-row>
        <v-col class="buttons">
          <v-btn
            title="Add note"
            @click="addNote"
            color="success"
          >Add note</v-btn>
          <v-btn
            title="Update notebook"
            @click="updateNotebook"
            color="success"
          >Update notebook</v-btn>
          <v-btn
            title="Delete notebook"
            @click="deleteNotebook"
            color="error"
          >Delete notebook</v-btn>
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <OverviewNote
            v-for="note of notes"
            :key="note.id"
            v-bind:note="note"
          ></OverviewNote>
        </v-col>
      </v-row>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { mapState } from "vuex";
import { Component, Vue, Watch } from "vue-property-decorator";
import OverviewNote from "@/components/OverviewNote.vue";
import Notebook from "../models/api/notebook";
import { resources } from "../resources";
import { LoadNotesQueryModel } from "../models/store/loadNotesQueryModel";

@Component({
  components: { OverviewNote },
  computed: mapState(["notes", "currentNotebook"])
})
export default class NotesList extends Vue {
  currentNotebook!: Notebook;

  constructor() {
    super();
  }

  mounted() {
    this.reloadData();
  }

  @Watch("$route")
  onRouteChanged() {
    this.reloadData();
  }

  updateNotebook() {
    this.$router.push({
      name: "updateNotebook",
      params: <any>{ id: this.$route.params.id }
    });
  }

  addNote() {
    this.$router.push({
      name: "addNote",
      query: <any>{ notebookId: this.currentNotebook.id }
    });
  }

  deleteNotebook() {
    if (confirm(resources.areYouSureDeleteNotebook)) {
      this.$store.dispatch("deleteNotebook", this.currentNotebook.id);
    }
  }

  private reloadData() {
    let query: LoadNotesQueryModel = {
      notebookId: parseInt(this.$route.params.id)
    };
    this.$store.dispatch("loadNotes", query);
    this.$store.dispatch("loadNotebook", this.$route.params.id);
  }
}
</script>

<style scoped>
</style>