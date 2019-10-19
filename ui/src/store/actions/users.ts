import urls from '@/urls';
import axios from 'axios';
import { ActionContext } from 'vuex';
import { StateModel } from '@/models/store/stateModel';
import { SignedInModel } from '@/models/api/signedInModel';
import { AuthenticateModel } from '@/models/api/authenticateModel';
import { errorMessage } from '@/utilities/messenger';
import { resources } from '@/resources';

export function authenticate(
    { commit }: ActionContext<StateModel, StateModel>,
    authenticateModel: AuthenticateModel) {
    axios.post(`${urls.rootUrl}api/user/authenticate`, authenticateModel) 
        .then(r => r.data)
        .then((signedInModel: SignedInModel) => {
            commit('SET_SIGNED_IN_USER', signedInModel)
        })
        .catch(error => {
            if(error && error.response.status === 401) {
                errorMessage(resources.credentialsIncorrect);
            }
        });
}