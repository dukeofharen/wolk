<template>
  <v-row>
    <v-col>
      <h1>Update notebook "{{notebook.name}}"</h1>
      <v-text-field
        label="Notebook name"
        type="text"
        v-model="notebook.name"
      ></v-text-field>
      <v-btn
        color="success"
        @click="updateNotebook"
      >Update notebook</v-btn>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { mapState } from "vuex";
import { Component, Vue, Watch } from "vue-property-decorator";
import { AuthenticateModel } from "../models/api/authenticateModel";
import { SignedInModel } from "../models/api/signedInModel";
import Notebook from "../models/api/notebook";

@Component({
  components: {},
  computed: mapState(["currentNotebook"])
})
export default class updateNotebook extends Vue {
  notebook: Notebook = {
    id: 0,
    name: ""
  };

  constructor() {
    super();
  }

  @Watch("currentNotebook")
  onPropertyChanged(value: Notebook) {
    this.notebook = value;
  }

  mounted() {
    this.$store.dispatch("loadNotebook", this.$route.params.id);
  }

  updateNotebook() {
    this.$store.dispatch("updateNotebook", {
      notebook: this.notebook,
      id: this.$route.params.id
    });
  }
}
</script>