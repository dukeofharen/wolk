import Vue from 'vue';
import Router from 'vue-router';

Vue.use(Router);

export default new Router({
  mode: 'hash',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      redirect: '/overview'
    },
    {
      path: '/notesList/:id',
      name: 'notesList',
      component: () => import(/* webpackChunkName: "notesList" */ './views/NotesList.vue'),
    },
    {
      path: '/login',
      name: 'login',
      component: () => import(/* webpackChunkName: "login" */ './views/Login.vue'),
    },
    {
      path: '/overview',
      name: 'overview',
      component: () => import(/* webpackChunkName: "overview" */ './views/Overview.vue'),
    },
    {
      path: '/notebookForm/:id?',
      name: 'notebookForm',
      component: () => import(/* webpackChunkName: "notebookForm" */ './views/NotebookForm.vue'),
    },
    {
      path: '/noteForm/:id?',
      name: 'noteForm',
      component: () => import(/* webpackChunkName: "noteForm" */ './views/NoteForm.vue'),
    },
    {
      path: '/viewNote/:id',
      name: 'viewNote',
      component: () => import(/* webpackChunkName: "viewNote" */ './views/ViewNote.vue'),
    },
  ],
});
