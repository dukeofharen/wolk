<template>
</template>

<script lang="ts">
import toastr from "toastr";
import { mapState } from "vuex";
import { Component, Vue, Watch } from "vue-property-decorator";
import Notebook from "../models/api/notebook";
import { MessageModel, MessageType } from "../models/store/messageModel";

@Component({
  components: {},
  computed: mapState(["message"])
})
export default class NotesList extends Vue {
  constructor() {
    super();
  }

  @Watch("message")
  onMessageChanged(messageModel: MessageModel) {
    switch (messageModel.type) {
      case MessageType.INFO:
        toastr.info(messageModel.message);
        break;
      case MessageType.SUCCESS:
        toastr.success(messageModel.message);
        break;
      case MessageType.WARNING:
        toastr.warning(messageModel.message);
        break;
      case MessageType.ERROR:
        toastr.error(messageModel.message);
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