<template>
    <div>{{contents}}</div>
</template>

<script lang="ts">
    import {Component, Vue, Watch, Prop} from "vue-property-decorator";
    import Note from "../../models/api/note";

    @Component({
        components: {}
    })
    export default class StickyNotes extends Vue {
        @Prop()
        contents!: string;
    }
</script>

<style scoped>
</style>