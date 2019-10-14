import axios from 'axios';
import { errorMessage } from '@/utilities/messenger';
import { resources } from '@/resources';
import store from '@/store/store';
import router from '@/router';

axios.interceptors.response.use(
    response => response,
    error => {
        if (error && error.response) {
            if (error.response.status === 400) {
                const messages: string[] = error.response.data;
                errorMessage(messages.join('\n'));
                return Promise.reject(error);
            } else if (error.response.status === 401) {
                store.commit("UNSET_SIGNED_IN_USER");
                errorMessage(resources.unauthorized, false);
                router.push({ name: 'login' });
                return Promise.reject(error);
            }
        }
    }
)