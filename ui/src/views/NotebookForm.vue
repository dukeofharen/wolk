<template>
  <v-row v-shortkey="['ctrl', 's']" @shortkey="saveNotebook">
    <v-col>
      <h1>{{notebookId ? "Update" : "Create"}} notebook</h1>
      <v-text-field
        label="Notebook name"
        type="text"
        v-model="notebook.name"
        @change="onChange"
      />
      <v-bottom-navigation color="indigo" fixed>
        <v-btn title="Save notebook" @click="saveNotebook">
          <v-icon>mdi-content-save</v-icon>
        </v-btn>
      </v-bottom-navigation>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { mapState } from "vuex";
import { Component, Vue, Watch } from "vue-property-decorator";
import Notebook from "../models/api/notebook";
import { resources } from "../resources";
import { UpdateNotebookCommand } from "../store/actions/notebooks";

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
      let command: UpdateNotebookCommand = {
        notebook: this.notebook,
        id: parseInt(this.$route.params.id)
      };
      this.$store.dispatch("updateNotebook", command);
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