<template>
  <div v-html="contents"></div>
</template>

<script lang="ts">
import { Component, Vue, Watch, Prop } from "vue-property-decorator";
import Note from "../../models/api/note";
import marked from "marked";

@Component({
  components: {}
})
export default class Markdown extends Vue {
  @Prop()
  note!: Note;
  contents: string;

  constructor() {
    super();
    this.contents = marked(this.note.content);
  }
}
</script>

<style scoped>
</style>