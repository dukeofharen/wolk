<template>
  <v-app id="keep">
    <v-app-bar app clipped-left color="amber">
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
      <span class="title ml-3 mr-5">
        Wolk&nbsp;
        <span class="font-weight-light">Notes</span>
      </span>
      <v-text-field solo-inverted flat hide-details label="Search" prepend-inner-icon="mdi-magnify"></v-text-field>
      <div class="flex-grow-1"></div>
    </v-app-bar>

    <v-navigation-drawer v-model="drawer" app clipped color="grey lighten-4">
      <NotebookBar></NotebookBar>
    </v-navigation-drawer>

    <v-content>
      <v-container fluid class="lighten-4">
        <router-view></router-view>
      </v-container>
    </v-content>
  </v-app>
</template>

<script>
import NotebookBar from '@/components/NotebookBar';
export default {

  components: {
      NotebookBar
  },
  data: () => ({
    drawer: null
  })
};
</script>
