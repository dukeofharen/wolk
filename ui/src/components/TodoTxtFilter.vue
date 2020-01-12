<template>
    <div>
        <v-row no-gutters>
            <v-col cols="12">
                <v-row>
                    <v-select
                            :items="projectTags"
                            placeholder="Filter on project tag..."
                            v-model="projectTag"
                            @change="filterChanged"
                            clearable
                    />
                </v-row>
            </v-col>
            <v-col cols="12">
                <v-row>
                    <v-select
                            :items="contextTags"
                            placeholder="Filter on context tag..."
                            v-model="contextTag"
                            @change="filterChanged"
                            clearable
                    />
                </v-row>
            </v-col>
        </v-row>
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
    import {TodoTxtModel} from "@/models/todoTxtModel";
    import {extractContextTags, extractProjectTags} from "@/services/todoTxtService";
    import {TodoTxtFilterModel} from "@/models/store/todoTxtFilterModel";

    @Component({
        components: {  }
    })
    export default class TodoTxtFilter extends Vue {
        projectTag: string = "";
        contextTag: string = "";
        
        @Prop()
        models!: TodoTxtModel[];
        
        filterChanged() {
            let filterModel: TodoTxtFilterModel = {
                projectTagFilter: this.projectTag,
                contextTagFilter: this.contextTag
            };
            this.$store.commit("SET_TODOTXT_FILTER", filterModel);
        }
        
        get projectTags() {
            return extractProjectTags(this.models);
        }

        get contextTags() {
            return extractContextTags(this.models);
        }
    }
</script>