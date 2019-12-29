<template>
    <div>{{contents}}</div>
</template>

<script lang="ts">
    import {Component, Vue, Watch, Prop} from "vue-property-decorator";
    import {stickyNotesToModel} from "@/services/stickyNoteService";

    @Component({
        components: {}
    })
    export default class StickyNotes extends Vue {
        @Prop()
        contents!: string;
        
        mounted() {
            console.log(stickyNotesToModel(this.contents));
        }
    }
</script>

<style scoped>
</style>