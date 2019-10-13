<template>
  <v-row >
    <v-col>
        <h1>Create notebook</h1>
        <v-text-field
              label="Notebook name"
              type="text"
              v-model="notebook.name"
            ></v-text-field>
        <v-btn color="success" @click="addNotebook">Create notebook</v-btn>
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
  components: {}
})
export default class AddNotebook extends Vue {
  notebook: Notebook = {
      id: 0,
      name: ""
  };

  constructor() {
    super();
  }

  addNotebook() {
    this.$store.dispatch('createNotebook', this.notebook);
  }
}
</script>