import axios from 'axios';
import store from '@/store/store';

axios.interceptors.response.use(response => {
    let token = response.headers['token'];
    if(token) {
        let signedInUser = store.state.signedInUser;
        signedInUser.token = token;
        store.commit("SET_SIGNED_IN_USER", signedInUser);
    }
    
    return response;
});