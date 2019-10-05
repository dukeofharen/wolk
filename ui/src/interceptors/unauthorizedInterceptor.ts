import axios from 'axios';
import { errorMessage } from '@/utilities/messenger';
import { resources } from '@/resources';
import router from '@/router';

axios.interceptors.response.use(
    response => response,
    error => {
        if (error && error.response && error.response.status === 401) {
            errorMessage(resources.unauthorized);
            router.push({ name: 'login' });
            return Promise.reject(error);
        }
    }
)