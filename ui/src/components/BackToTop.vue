<template>
    <v-btn title="Back to top" @click="backToTop">
        <v-icon>mdi-arrow-up</v-icon>
    </v-btn>
</template>

<script lang="ts">
    import {Component, Vue} from "vue-property-decorator";
    import {backToTop} from "@/utilities/windowHelper";

    @Component
    export default class BackToTop extends Vue {
        backToTop() {
            backToTop();
        }
    }
</script>