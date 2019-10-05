import { StateModel } from '@/models/store/stateModel';
import { SignedInModel } from '@/models/api/signedInModel';

export function SET_SIGNED_IN_USER(state: StateModel, signedInUser: SignedInModel) {
    state.signedInUser = signedInUser;
}