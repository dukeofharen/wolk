<template>
    <div v-html="renderedContents"></div>
</template>

<script lang="ts">
    import {Component, Vue, Watch, Prop} from "vue-property-decorator";
    import marked from "marked";

    @Component({
        components: {}
    })
    export default class Markdown extends Vue {
        @Prop()
        contents!: string;

        renderedContents: string = "";

        @Watch("contents")
        onContentsChanged() {
            this.renderContents();
        }

        mounted() {
            this.renderContents();
        }

        private renderContents() {
            this.renderedContents = marked(this.contents);
        }
    }
</script>

<style scoped>
</style>