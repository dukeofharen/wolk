import { SignedInModel } from '@/models/api/signedInModel';
import { StateModel } from '@/models/store/stateModel';

export function signedInUser(state: StateModel): SignedInModel {
    return state.signedInUser;
}