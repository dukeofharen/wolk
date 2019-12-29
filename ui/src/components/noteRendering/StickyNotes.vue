<template>
    <v-container class="lighten-5">
        <v-row no-gutters>
            <v-col cols="12" sm="4" v-for="model of models" :key="model.title">
                <v-card class="pa-2 sticky-note" tile>
                    <v-card-title>{{model.title}}</v-card-title>
                    <v-card-text>{{model.contents}}</v-card-text>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>

<script lang="ts">
    import {Component, Vue, Prop} from "vue-property-decorator";
    import {stickyNotesToModel} from "@/services/stickyNoteService";
    import {StickyNotesModel} from "@/models/stickyNotesModel";

    @Component({
        components: {}
    })
    export default class StickyNotes extends Vue {
        @Prop()
        contents!: string;

        models: StickyNotesModel[] = [];

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