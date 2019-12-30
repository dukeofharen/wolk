<template>
    <div>{{contents}}</div>
</template>

<script lang="ts">
    import {Component, Prop, Vue} from "vue-property-decorator";
    import {StickyNotesModel} from "@/models/stickyNotesModel";
    import marked from "marked";
    import Note from "@/models/api/note";
    import {todoTxtToModels} from "@/services/todoTxtService";

    @Component({
        components: {}
    })
    export default class StickyNotes extends Vue {
        @Prop()
        contents!: string;

        @Prop()
        note!: Note;

        models: StickyNotesModel[] = [];
        indexEditing: number = -1;

        data() {
            return {
                marked: marked
            }
        }

        mounted() {
            console.log(todoTxtToModels(this.contents));
            // this.models = stickyNotesToModel(this.contents);
        }

        saveNote() {
        }
    }
</script>

<style scoped>
    .sticky-note {
        margin: 10px;
    }

    .sticky-title, .sticky-contents {
        cursor: pointer;
    }

    .sticky-title > div {
        width: 100%;
    }

    .sticky-contents textarea {
        width: 100%;
        min-height: 200px;
    }

    .sticky-note .v-btn {
        min-width: 0;
        width: 35px;
    }

    .color-buttons .v-btn {
        border-radius: 30px;
        margin-left: 10px !important;
        margin-top: 10px !important;
    }
</style>