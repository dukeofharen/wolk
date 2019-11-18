<template>
    <v-btn
            fixed
            dark
            fab
            bottom
            right
            color="pink"
            title="Back to top"
            @click="backToTop"
    >
        <v-icon>mdi-arrow-up</v-icon>
    </v-btn>
</template>

<script lang="ts">
    import {Component, Vue} from "vue-property-decorator";

    @Component
    export default class ScrollToTop extends Vue {
        
        get showButton(): boolean {
            return document.body.scrollTop > 20 || document.documentElement.scrollTop > 20;
        }
        
        backToTop() {
            document.body.scrollTop = 0;
            document.documentElement.scrollTop = 0;
        }
    }
</script>