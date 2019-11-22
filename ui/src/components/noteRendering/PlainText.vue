<template>
    <div v-html="renderedContents"></div>
</template>

<script lang="ts">
    import {Component, Vue, Watch, Prop} from "vue-property-decorator";
    import Note from "../../models/api/note";

    @Component({
        components: {}
    })
    export default class PlainText extends Vue {
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
            // https://stackoverflow.com/questions/18749591/encode-html-entities-in-javascript
            this.renderedContents = this.contents
                .replace(/[\u00A0-\u9999<>\&]/gim, function (i) {
                    return "&#" + i.charCodeAt(0) + ";";
                })
                .replace(/(?:\r\n|\r|\n)/g, "<br />");
        }
    }
</script>

<style scoped>
</style>