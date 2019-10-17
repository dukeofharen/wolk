<template>
  <v-row>
    <v-col>
      <h1>Create note</h1>
      <v-text-field
        label="Note title"
        type="text"
        v-model="note.title"
      ></v-text-field>
      <v-select
        :items="notebooks"
        placeholder="Select notebook..."
        v-model="note.notebookId"
        item-text="name"
        item-value="id"
        clearable
      ></v-select>
      <v-select
        :items="noteTypeNames"
        placeholder="Select note type..."
        v-model="note.noteType"
        item-text="value"
        item-value="key"
        clearable
      ></v-select>
      <v-textarea
        label="Note contents"
        v-model="note.content"
        auto-grow
      ></v-textarea>
      <v-btn
        color="success"
        @click="addNote"
      >Create note</v-btn>
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
import { getNoteTypeArray, NoteType } from "../models/api/enums/noteType";
import { KeyValuePair } from "../models/keyValuePair";

@Component({
  components: {},
  computed: mapState(["notebooks"])
})
export default class AddNote extends Vue {
  noteTypeNames: Array<KeyValuePair<NoteType, string>> = getNoteTypeArray();
  notebooks!: Notebook[];
  note: Note = {
    id: 0,
    title: "",
    content: "",
    notebookId: 0,
    preview: "",
    noteType: NoteType.NotSet,
    created: new Date(),
    updated: new Date()
  };

  constructor() {
    super();
  }

  mounted() {
    let notebookId = <string>this.$route.query.notebookId;
    if (notebookId) {
      this.note.notebookId = parseInt(notebookId);
    }
  }

  addNote() {
    this.$store.dispatch("createNote", this.note);
  }
}
</script>