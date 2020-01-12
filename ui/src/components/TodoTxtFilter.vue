<template>
    <div>
        <v-row no-gutters class="filter">
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
            <v-col cols="12">
                <v-row>
                    <v-switch
                            v-model="excludeDone"
                            @change="filterChanged"
                            label="Exclude finished todo items" />
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
        excludeDone: boolean = false;
        
        @Prop()
        models!: TodoTxtModel[];
        
        created() {
            let filter = this.$store.getters.todoTxtFilter;
            this.projectTag = filter.projectTagFilter;
            this.contextTag = filter.contextTagFilter;
            this.excludeDone = filter.excludeDone;
        }
        
        filterChanged() {
            let filterModel: TodoTxtFilterModel = {
                projectTagFilter: this.projectTag,
                contextTagFilter: this.contextTag,
                excludeDone: this.excludeDone
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

<style scoped>
    .filter {
        margin-left: 10px;
        margin-right: 10px;
    }
</style>