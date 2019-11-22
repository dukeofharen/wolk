<template>
  <div>
    <PlainText v-if="noteType === NoteType.PlainText" :contents="contents" />
    <Markdown v-if="noteType === NoteType.Markdown" :contents="contents" />
    <div v-if="noteType === NoteType.NotSet">
      {{contents}}
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import { NoteType } from "@/models/api/enums/noteType";
import PlainText from "@/components/noteRendering/PlainText.vue";
import Markdown from "@/components/noteRendering/Markdown.vue";

@Component({
  components: { PlainText, Markdown }
})
export default class NoteRender extends Vue {
  NoteType = NoteType;

  @Prop()
  contents!: string;

  @Prop()
  noteType!: NoteType;

  constructor() {
    super();
  }
}
</script>