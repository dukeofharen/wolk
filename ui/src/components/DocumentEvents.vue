<template>

</template>

<script lang="ts">
    import {Component, Vue, Watch} from "vue-property-decorator";
    import {mapState} from "vuex";

    @Component({
        computed: mapState(["pageSubTitle"])
    })
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
        
        @Watch("pageSubTitle")
        onPageSubTitleChanged(newValue: string) {
            console.log(newValue);
        }
    }
</script>

<style scoped>

</style>