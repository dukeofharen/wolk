import store from '@/store/store';
import {SignedInModel} from '@/models/api/signedInModel';

export default function addJwtToRequest(request: any) {
    let signedInUser: SignedInModel = store.getters.signedInUser;
    if (signedInUser && !!signedInUser.token) {
        request.headers['Authorization'] = `Bearer ${signedInUser.token}`;
    }

    return request;
}