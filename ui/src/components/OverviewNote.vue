<template>
  <v-row>
    <v-col>
      <v-card @click="viewNote">
        <v-card-title>{{note.title}}</v-card-title>
        <v-card-text>
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
            <v-chip title="Note type">
              <v-icon left>mdi-book-variant</v-icon>{{noteTypeName}}
            </v-chip>
          </div>
          <div>
            {{note.preview}}
          </div>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Component, Vue, Watch, Prop } from "vue-property-decorator";
import Notebook from "../models/api/notebook";
import Note from "../models/api/note";
import { NoteTypeMap } from "../models/api/enums/noteType";

@Component({
  components: {}
})
export default class OverviewNote extends Vue {
  @Prop()
  note!: Note;
  noteTypeName: string = "";

  constructor() {
    super();
    this.noteTypeName = NoteTypeMap.get(this.note.noteType);
  }

  viewNote() {
    this.$router.push({ name: "viewNote", params: <any>{ id: this.note.id } });
  }
}
</script>

<style scoped>
</style>