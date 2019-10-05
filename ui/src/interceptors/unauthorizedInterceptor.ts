import axios from 'axios';
import { errorMessage } from '@/utilities/messenger';

axios.interceptors.response.use(
    response => response,
    error => {
        if(error.response.status === 401) {
            errorMessage('UNAUTHORIZED!');
            return Promise.reject(error);
        }
    }
)