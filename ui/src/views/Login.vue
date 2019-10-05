<template>
  <v-row v-on:keyup.enter="logIn">
    <v-col>
      <v-card class="elevation-12">
        <v-card-text>
          <v-form>
            <v-text-field
              label="Login"
              name="login"
              prepend-icon="mdi-account"
              type="text"
              v-model="username"
            ></v-text-field>
            <v-text-field
              id="password"
              label="Password"
              name="password"
              prepend-icon="mdi-lock"
              type="password"
              v-model="password"
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn color="primary" @click="logIn">Login</v-btn>
        </v-card-actions>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { mapState } from "vuex";
import { Component, Vue, Watch } from "vue-property-decorator";

@Component({
  components: {}
})
export default class Login extends Vue {
  constructor() {
    super();
  }

  data() {
    return {
      username: '',
      password: ''
    };
  }

  logIn() {
    console.log('LOGIN')
  }
}
</script>
</script>