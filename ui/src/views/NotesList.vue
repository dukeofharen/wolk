<template>
  <v-row>
    <v-col>
      <h1>{{currentNotebook.name}}</h1>
      <OverviewNote v-for="note of notes" :key="note.id" v-bind:note="note"></OverviewNote>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { mapState } from "vuex";
import { Component, Vue, Watch } from "vue-property-decorator";
import OverviewNote from "@/components/OverviewNote.vue";
import Notebook from "../models/api/notebook";

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
  onPropertyChanged(value: any, oldValue: any) {
    this.reloadData();
  }

  private reloadData() {
    this.$store.dispatch("loadNotes", this.$route.params.id);
    this.$store.dispatch("loadNotebook", this.$route.params.id);
  }
}
</script>

<style scoped>
</style>