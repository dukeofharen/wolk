import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store/store';
import './registerServiceWorker';
import vuetify from './plugins/vuetify';

// Axios interceptors
import '@/interceptors/unauthorizedInterceptor';

// Fonts
import 'typeface-roboto';

// Toastr
import 'toastr/build/toastr.css';

// Style
import '@/css/style.css';

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  vuetify,
  render: (h) => h(App),
}).$mount('#app');
