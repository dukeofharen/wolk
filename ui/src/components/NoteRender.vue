<template>
  <div>
    <PlainText v-if="noteType === NoteType.PlainText" :contents="contents" />
    <Markdown v-if="noteType === NoteType.Markdown" :contents="contents" />
    <StickyNotes v-if="noteType === NoteType.StickyNotes" :contents="contents" :note="note" />
    <TodoTxt v-if="noteType === NoteType.TodoTxt" :contents="contents" :note="note" />
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
import StickyNotes from "@/components/noteRendering/StickyNotes.vue";
import TodoTxt from "@/components/noteRendering/TodoTxt.vue";
import Note from "@/models/api/note";

@Component({
  components: { PlainText, Markdown, StickyNotes, TodoTxt }
})
export default class NoteRender extends Vue {
  NoteType = NoteType;

  @Prop()
  contents!: string;

  @Prop()
  noteType!: NoteType;
  
  @Prop()
  note!: Note;

  constructor() {
    super();
  }
}
</script>