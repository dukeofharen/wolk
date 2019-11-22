<template>
  <v-row>
    <v-col>
      <h1>{{note.title}}</h1>
      <div>
        <v-chip title="Date/time created">
          <v-icon left>mdi-clock</v-icon>{{note.created | datetime}}
        </v-chip>
        <v-chip
          title="Date/time updated"
          v-if="note.changed"
        >
          <v-icon left>mdi-clock</v-icon>{{note.changed | datetime}}
        </v-chip>
      </div>
      <v-row>
        <v-col class="buttons">
          <v-btn
            title="Update note"
            :to="{ name: 'noteForm', params: {id: note.id}}"
            color="success"
          >Update note</v-btn>
          <v-btn
            title="Attachments"
            @click="showAttachments"
            color="success"
          >Attachments</v-btn>
          <v-btn
            title="Delete note"
            @click="deleteNote"
            color="error"
          >Delete note</v-btn>
        </v-col>
      </v-row>
      <Attachments :noteId="note.id" v-if="attachmentsOpened" />
      <NoteRender
        :contents="note.content"
        :noteType="note.noteType"
      />
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
import { resources } from "../resources";
import { NoteType } from "../models/api/enums/noteType";
import NoteRender from "@/components/NoteRender.vue";
import Attachments from "@/components/Attachments.vue";

@Component({
  components: { NoteRender, Attachments },
  computed: mapState(["currentNote"])
})
export default class ViewNote extends Vue {
  NoteType = NoteType;

  attachmentsOpened: boolean = false;
  note: Note = {
    id: 0,
    title: "",
    content: "",
    notebookId: 0,
    preview: "",
    noteType: NoteType.NotSet,
    created: new Date(),
    updated: new Date(),
    opened: new Date()
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
      name: "noteForm",
      params: <any>{ id: this.note.id }
    });
  }

  deleteNote() {
    if (confirm(resources.areYouSureDeleteNote)) {
      this.$store.dispatch("deleteNote", this.note);
    }
  }

  showAttachments() {
    this.attachmentsOpened = !this.attachmentsOpened;
  }

  private reloadData() {
    this.$store.dispatch("loadNote", this.$route.params.id);
  }
}
</script>