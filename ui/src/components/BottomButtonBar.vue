<template>
    <v-btn
            fixed
            dark
            fab
            bottom
            right
            color="pink"
            title="Back to top"
            v-if="!uiState.isOnTop"
            @click="backToTop"
    >
        <v-icon>mdi-arrow-up</v-icon>
    </v-btn>
</template>

<script lang="ts">
    import {Component, Vue} from "vue-property-decorator";
    import {mapState} from "vuex";
    import {UiStateModel} from "@/models/store/uiStateModel";

    @Component({
        computed: mapState(["uiState"])
    })
    export default class BottomButtonBar extends Vue {
        uiState!: UiStateModel;
        
        backToTop() {
            document.body.scrollTop = 0;
            document.documentElement.scrollTop = 0;
        }
    }
</script>