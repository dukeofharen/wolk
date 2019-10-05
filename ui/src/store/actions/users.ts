import urls from '@/urls';
import axios from 'axios';
import { ActionContext } from 'vuex';
import { StateModel } from '@/models/store/stateModel';
import { SignedInModel } from '@/models/api/signedInModel';
import { AuthenticateModel } from '@/models/api/authenticateModel';

export function authenticate(
    { commit }: ActionContext<StateModel, StateModel>,
    authenticateModel: AuthenticateModel) {
    axios.post(`${urls.rootUrl}api/v1/users`, authenticateModel)
        .then(r => r.data)
        .then((signedInModel: SignedInModel) => {
            commit('SET_SIGNED_IN_USER', signedInModel)
        });
}