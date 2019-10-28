import {StateModel} from '@/models/store/stateModel';
import {SignedInModel} from '@/models/api/signedInModel';
import {setLocalValue, unsetLocalValue} from '@/data/localDataHelper';
import {keys} from '@/resources';

export function SET_SIGNED_IN_USER(state: StateModel, signedInUser: SignedInModel) {
    state.signedInUser = signedInUser;
    setLocalValue(keys.signedInUserKey, signedInUser);
}

export function UNSET_SIGNED_IN_USER(state: StateModel) {
    state.signedInUser = {
        email: '',
        id: 0,
        token: ''
    };
    unsetLocalValue(keys.signedInUserKey);
}