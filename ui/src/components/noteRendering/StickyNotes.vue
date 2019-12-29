<template>
    <v-container class="lighten-5">
        <v-row no-gutters>
            <v-col cols="12" sm="4" v-for="(model, i) of models" :key="i">
                <v-card class="pa-2 sticky-note" tile>
                    <v-card-title>{{model.title}}</v-card-title>
                    <v-card-text><div v-html="marked(model.contents)" /></v-card-text>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>

<script lang="ts">
    import {Component, Vue, Prop} from "vue-property-decorator";
    import {stickyNotesToModel} from "@/services/stickyNoteService";
    import {StickyNotesModel} from "@/models/stickyNotesModel";
    import marked from "marked";

    @Component({
        components: {}
    })
    export default class StickyNotes extends Vue {
        @Prop()
        contents!: string;

        models: StickyNotesModel[] = [];
        
        data() {
            return{
                marked: marked
            }
        }

        mounted() {
            this.models = stickyNotesToModel(this.contents);
        }
    }
</script>

<style scoped>
    .sticky-note {
        margin: 10px;
    }
</style>