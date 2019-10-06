<template>
  <v-row>
    <v-col>
      <v-card>
        <v-card-title>{{note.title}}</v-card-title>
        <v-card-text>{{note.preview}}</v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Component, Vue, Watch, Prop } from "vue-property-decorator";
import Notebook from "../models/api/notebook";
import Note from "../models/api/note";

@Component({
  components: {}
})
export default class OverviewNote extends Vue {
  @Prop()
  note!: Note;

  constructor() {
    super();
  }
}
</script>

<style scoped>
</style>