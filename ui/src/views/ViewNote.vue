<template>
  <v-row>
    <v-col>
      <h1>{{note.title}}</h1>
      <v-chip title="Date/time created">
        <v-icon left>mdi-clock</v-icon>{{note.created | datetime}}
      </v-chip>
      <v-chip
        title="Date/time updated"
        v-if="note.changed"
      >
        <v-icon left>mdi-clock</v-icon>{{note.changed | datetime}}
      </v-chip>
      <v-row>
        <v-col class="buttons">
          <v-btn
            title="Update note"
            @click="updateNote"
            color="success"
          >Update note</v-btn>
        </v-col>
      </v-row>
      <div>
        {{note.content}}
      </div>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { mapState } from "vuex";
import { Component, Vue, Watch } from "vue-property-decorator";
import { AuthenticateModel } from "../models/api/authenticateModel";
import { SignedInModel } from "../models/api/signedInModel";
import Note from "../models/api/note";
import Notebook from "../models/api/notebook";

@Component({
  components: {},
  computed: mapState(["currentNote"])
})
export default class ViewNote extends Vue {
  note: Note = {
    id: 0,
    title: "",
    content: "",
    notebookId: 0,
    preview: "",
    created: null,
    updated: null
  };

  constructor() {
    super();
  }

  mounted() {
    this.reloadData();
  }

  @Watch("currentNote")
  onCurrentNoteChanged(value: Note) {
    this.note = value;
  }

  @Watch("$route")
  onRouteChanged() {
    this.reloadData();
  }

  updateNote() {
    this.$router.push({
      name: "updateNote",
      params: <any>{ id: this.note.id }
    });
  }

  private reloadData() {
    this.$store.dispatch("loadNote", this.$route.params.id);
  }
}
</script>