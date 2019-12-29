import { errorMessage } from '@/utilities/messenger';
import { resources } from '@/resources';
import store from '@/store/store';
import router from '@/router';

export default function handleError(error: any) {
    if (error && error.response) {
        if (error.response.status === 400) {
            const messages: string[] = error.response.data;
            errorMessage(messages.join('\n'));
        } else if (error.response.status === 401) {
            store.commit("UNSET_SIGNED_IN_USER");
            router.push({ name: 'login' });
        } else if (error.response.status === 404) {
            errorMessage(resources.notFound);
            router.push({ name: 'overview' });
        } else {
            console.log(error);
            errorMessage(resources.serverError);
        }

        return Promise.reject(error);
    }
}