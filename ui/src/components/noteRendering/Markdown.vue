<template>
  <div v-html="renderedContents"></div>
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
  contents: string;

  renderedContents: string = "";

  constructor() {
    super();
    this.renderedContents = marked(this.contents);
  }
}
</script>

<style scoped>
</style>