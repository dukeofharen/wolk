<template>
  <v-row>
    <v-col>
      <v-card
        v-for="note of notes"
        :key="note.id"
      >
        <v-card-title>{{note.title}}</v-card-title>
        <v-card-text>{{note.preview}}</v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { mapState } from "vuex";
import { Component, Vue, Watch } from "vue-property-decorator";
import Notebook from "../models/api/notebook";

@Component({
  components: {},
  computed: mapState(["notes"])
})
export default class NotesList extends Vue {
  constructor() {
    super();
  }

  mounted() {
    this.loadNotes();
  }

  @Watch("$route")
  onPropertyChanged(value: any, oldValue: any) {
    this.loadNotes();
  }

  private loadNotes() {
    this.$store.dispatch("loadNotes", this.$route.params.id);
  }
}
</script>

<style scoped>
.v-card {
  margin-bottom: 10px;
}
</style>