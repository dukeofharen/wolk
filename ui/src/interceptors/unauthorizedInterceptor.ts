import axios from 'axios';
import { errorMessage } from '@/utilities/messenger';
import { resources } from '@/resources';

axios.interceptors.response.use(
    response => response,
    error => {
        if (error.response.status === 401) {
            errorMessage(resources.unauthorized);
            return Promise.reject(error);
        }
    }
)