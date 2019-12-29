import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store/store';
import './registerServiceWorker';
import vuetify from './plugins/vuetify';
import { keys } from './resources';
import { getLocalValue } from './data/localDataHelper';

// Fonts
import 'typeface-roboto';

// Style
import '@/css/style.css';
import { SignedInModel } from './models/api/signedInModel';

// Filters
import '@/filters/datetime';
import '@/filters/filesize';

// Other stuff
import '@/utilities/primitiveOverrides';

// Other Vue components
Vue.use(require('vue-shortkey'));

Vue.config.productionTip = false;

// Check local storage if user has already signed in before.
let signedInUser: SignedInModel = getLocalValue(keys.signedInUserKey);
if (signedInUser) {
  store.commit('SET_SIGNED_IN_USER', signedInUser);
}

new Vue({
  router,
  store,
  vuetify,
  render: (h) => h(App),
}).$mount('#app');
