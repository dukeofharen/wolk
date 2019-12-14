<template>
    <v-snackbar v-model="showSnackbar" :color="color">
        <span>{{message.message}}</span>
        <v-btn text @click="showSnackbar = false">Close</v-btn>
    </v-snackbar>
</template>

<script lang="ts">
    import toastr from "toastr";
    import {mapState} from "vuex";
    import {Component, Vue, Watch} from "vue-property-decorator";
    import Notebook from "../models/api/notebook";
    import {MessageModel, MessageType} from "../models/store/messageModel";

    @Component({
        components: {},
        computed: mapState(["message"])
    })
    export default class NotesList extends Vue {
        showSnackbar: boolean = false;
        color: string = "";

        constructor() {
            super();
        }

        @Watch("message")
        onMessageChanged(messageModel: MessageModel) {
            this.showSnackbar = true;
            switch (messageModel.type) {
                case MessageType.INFO:
                    this.color = "info";
                    break;
                case MessageType.SUCCESS:
                    this.color = "success";
                    break;
                case MessageType.WARNING:
                    this.color = "warning";
                    break;
                case MessageType.ERROR:
                    this.color = "error";
                    break;
            }
        }
    }
</script>

<style scoped>
    .v-card {
        margin-bottom: 10px;
    }
</style>