import axios from 'axios';
import { errorMessage } from '@/utilities/messenger';
import { resources, keys } from '@/resources';
import { unsetLocalValue } from '@/data/localDataHelper';
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
                unsetLocalValue(keys.signedInUserKey);
                errorMessage(resources.unauthorized);
                router.push({ name: 'login' });
                return Promise.reject(error);
            }
        }
    }
)