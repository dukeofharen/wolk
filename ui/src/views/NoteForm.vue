<template>
  <v-row>
    <v-col>
      <h1>{{noteId ? "Update" : "Create"}} note</h1>
      <v-text-field
        label="Note title"
        type="text"
        v-model="note.title"
        @change="onChange"
      ></v-text-field>
      <v-select
        :items="notebooks"
        placeholder="Select notebook..."
        v-model="note.notebookId"
        item-text="name"
        item-value="id"
        clearable
        @change="onChange"
      ></v-select>
      <v-select
        :items="noteTypeNames"
        placeholder="Select note type..."
        v-model="note.noteType"
        item-text="value"
        item-value="key"
        clearable
        @change="onChange"
      ></v-select>
      <v-textarea
        label="Note contents"
        v-model="note.content"
        auto-grow
        @change="onChange"
      ></v-textarea>
      <v-btn
        color="success"
        @click="saveNote"
      >Save note</v-btn>
      <v-btn
        color="success"
        @click="viewNote"
        v-if="noteId"
      >View note</v-btn>
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
import { resources } from "../resources";

@Component({
  components: {},
  computed: mapState(["notebooks", "currentNote"]),
  beforeRouteLeave(to, from, next) {
    if (!this.formDirty || confirm(resources.unsavedChanges)) {
      next();
    }
  }
})
export default class NoteForm extends Vue {
  noteId: string = "";
  noteTypeNames: Array<KeyValuePair<NoteType, string>> = getNoteTypeArray();
  notebooks!: Notebook[];
  note: Note = this.emptyNote;
  formDirty: boolean = false;

  constructor() {
    super();
  }

  @Watch("$route")
  onRouteChanged() {
    this.reloadData();
  }

  @Watch("currentNote")
  onCurrentNoteChanged(value: Note) {
    this.note = value;
  }

  mounted() {
    this.reloadData();
    this.formDirty = false;
    let notebookId = <string>this.$route.query.notebookId;
    if (notebookId) {
      this.note.notebookId = parseInt(notebookId);
    }
  }

  onChange() {
    this.formDirty = true;
  }

  saveNote() {
    if (!this.noteId) {
      this.$store.dispatch("createNote", this.note);
    } else {
      this.$store.dispatch("updateNote", {
        note: this.note,
        id: this.noteId
      });
    }

    this.formDirty = false;
  }

  viewNote() {
    this.$router.push({ name: "viewNote", params: <any>{ id: this.noteId } });
  }

  private reloadData() {
    this.noteId = <string>this.$route.params.id;
    if (!this.noteId) {
      this.note = this.emptyNote;
    } else {
      this.$store.dispatch("loadNote", this.noteId);
    }
  }

  get emptyNote(): Note {
    return {
      id: 0,
      title: "",
      content: "",
      notebookId: 0,
      preview: "",
      noteType: NoteType.NotSet,
      created: new Date(),
      updated: new Date()
    };
  }
}
</script>