<template>
  <v-row>
    <v-col>
      <h1>Overview</h1>
      <OverviewNote
        v-for="note of notes"
        :key="note.id"
        v-bind:note="note"
      ></OverviewNote>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { mapState } from "vuex";
import { Component, Vue, Watch } from "vue-property-decorator";
import OverviewNote from "@/components/OverviewNote.vue";

@Component({
  components: { OverviewNote },
  computed: mapState(["notes"])
})
export default class Overview extends Vue {
  constructor() {
    super();
  }

  mounted() {
    this.loadNotes();
  }

  private loadNotes() {
    this.$store.dispatch("loadNotes");
  }
}
</script>

<style scoped>
</style>