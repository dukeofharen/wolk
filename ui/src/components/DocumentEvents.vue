<template>

</template>

<script lang="ts">
    import {Component, Vue} from "vue-property-decorator";

    @Component
    export default class DocumentEvents extends Vue {
        constructor() {
            super();
            window.document.addEventListener('scroll', e => {
                let isOnTop = window.scrollY === 0;
                if (isOnTop !== this.$store.state.uiState.isOnTop) {
                    this.$store.commit('SET_IS_ON_TOP', isOnTop);
                }
            });
        }
    }
</script>

<style scoped>

</style>