<template>
  <v-row v-shortkey="['ctrl', 's']" @shortkey="saveNotebook">
    <v-col>
      <h1>{{notebookId ? "Update" : "Create"}} notebook</h1>
      <v-text-field
        label="Notebook name"
        type="text"
        v-model="notebook.name"
        @change="onChange"
      ></v-text-field>
      <v-btn
        color="success"
        @click="saveNotebook"
      >Save notebook</v-btn>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { mapState } from "vuex";
import { Component, Vue, Watch } from "vue-property-decorator";
import { AuthenticateModel } from "../models/api/authenticateModel";
import { SignedInModel } from "../models/api/signedInModel";
import Notebook from "../models/api/notebook";
import { resources } from "../resources";

@Component({
  components: {},
  computed: mapState(["currentNotebook"]),
  beforeRouteLeave(to, from, next) {
    let form: NotebookForm = this as NotebookForm;
    if (!form.formDirty || confirm(resources.unsavedChanges)) {
      next();
    }
  }
})
export default class NotebookForm extends Vue {
  notebookId: string = "";
  notebook: Notebook = this.emptyNotebook();
  formDirty: boolean = false;

  constructor() {
    super();
  }

  @Watch("currentNotebook")
  onCurrentNotebookChanged(value: Notebook) {
    this.notebook = value;
  }

  @Watch("$route")
  onRouteChanged() {
    this.reloadData();
  }

  mounted() {
    this.formDirty = false;
    this.reloadData();
  }

  saveNotebook() {
    if (!this.notebookId) {
      this.$store.dispatch("createNotebook", this.notebook);
    } else {
      this.$store.dispatch("updateNotebook", {
        notebook: this.notebook,
        id: this.$route.params.id
      });
    }

    this.formDirty = false;
  }

  onChange() {
    this.formDirty = true;
  }

  private reloadData() {
    this.notebookId = <string>this.$route.params.id;
    if (!this.notebookId) {
      this.notebook = this.emptyNotebook();
    } else {
      this.$store.dispatch("loadNotebook", this.$route.params.id);
    }
  }

  emptyNotebook(): Notebook {
    return {
      id: 0,
      name: "",
      created: new Date(),
      updated: new Date()
    };
  }
}
</script>