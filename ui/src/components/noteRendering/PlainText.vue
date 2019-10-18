<template>
  <div v-html="contents"></div>
</template>

<script lang="ts">
import { Component, Vue, Watch, Prop } from "vue-property-decorator";
import Note from "../../models/api/note";

@Component({
  components: {}
})
export default class PlainText extends Vue {
  @Prop()
  note!: Note;
  contents: string;

  constructor() {
    super();
    this.contents = this.note.content.replace(/(?:\r\n|\r|\n)/g, '<br />');
  }
}
</script>

<style scoped>
</style>