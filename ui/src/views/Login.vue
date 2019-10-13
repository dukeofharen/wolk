<template>
  <v-row v-on:keyup.enter="logIn">
    <v-col>
      <v-card class="elevation-12">
        <v-card-text>
          <v-form>
            <v-text-field
              label="Email address"
              name="email"
              prepend-icon="mdi-account"
              type="text"
              v-model="authenticateModel.email"
            ></v-text-field>
            <v-text-field
              id="password"
              label="Password"
              name="password"
              prepend-icon="mdi-lock"
              type="password"
              v-model="authenticateModel.password"
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn
            color="primary"
            @click="logIn"
          >Login</v-btn>
        </v-card-actions>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";
import { AuthenticateModel } from "../models/api/authenticateModel";
import { SignedInModel } from "../models/api/signedInModel";

@Component({
  components: {}
})
export default class Login extends Vue {
  authenticateModel: AuthenticateModel = {
    email: "",
    password: ""
  };

  constructor() {
    super();
  }

  mounted() {
    this.handleLogin();
  }

  get signedInUser(): SignedInModel {
    return this.$store.state.signedInUser;
  }

  get isSignedIn(): boolean {
    return this.$store.getters.isSignedIn;
  }

  @Watch("signedInUser")
  loginChanged() {
    this.handleLogin();
  }

  logIn() {
    this.$store.dispatch("authenticate", this.authenticateModel);
  }

  private handleLogin() {
    if (this.$store.getters.isSignedIn) {
      this.$router.push({ name: "overview" });
    }
  }
}
</script>