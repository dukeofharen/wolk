<template>
    <v-snackbar v-model="showSnackbar" :color="color" multi-line @click="executeCallback" :class="cssClass" top>
        <span v-if="!multipleMessages">{{message.message}}</span>
        <span v-if="multipleMessages">
            <template v-for="text of message.message">{{text}}<br/></template>
        </span>
        <v-btn text @click="showSnackbar = false">Close</v-btn>
    </v-snackbar>
</template>

<script lang="ts">
    import {mapState} from "vuex";
    import {Component, Vue, Watch} from "vue-property-decorator";
    import {MessageModel, MessageType} from "@/models/store/messageModel";

    @Component({
        components: {},
        computed: mapState(["message"])
    })
    export default class NotesList extends Vue {
        showSnackbar: boolean = false;
        multipleMessages: boolean = false;
        color: string = "";
        cssClass: string = "";
        message!: MessageModel;

        constructor() {
            super();
        }

        @Watch("message")
        onMessageChanged(messageModel: MessageModel) {
            this.showSnackbar = true;
            this.multipleMessages = Array.isArray(messageModel.message);
            this.cssClass = !!messageModel.callback ? "clickable" : "";
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

        executeCallback() {
            if (this.message.callback && typeof this.message.callback === "function") {
                this.message.callback();
                this.showSnackbar = false;
            }
        }
    }
</script>

<style scoped>
    .clickable {
        cursor: pointer;
    }
</style>