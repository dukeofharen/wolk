<template>
  <v-row>
    <v-col>
        OVERVIEW
      <!-- <v-card
        v-for="note of notes"
        :key="note.id"
      >
        <v-card-title>{{note.title}}</v-card-title>
        <v-card-text>{{note.preview}}</v-card-text>
      </v-card> -->
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { mapState } from "vuex";
import { Component, Vue, Watch } from "vue-property-decorator";

@Component({
  components: {},
//   computed: mapState(["notes"])
})
export default class Overview extends Vue {
  constructor() {
    super();
  }
}
</script>

<style scoped>
</style>