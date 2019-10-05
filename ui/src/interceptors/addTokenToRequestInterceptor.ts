import axios from 'axios';
import store from '@/store/store';
import { SignedInModel } from '@/models/api/signedInModel';

axios.interceptors.request.use(
    request => {
        let signedInUser: SignedInModel = store.getters.signedInUser;
        if(!!signedInUser.token) {
            request.headers['Authorization'] = `Bearer ${signedInUser.token}`;
        }

        return request;
    }
)