import store from '@/store/store';

export default function handleJwtRenewal(response: any) {
    let token = response.headers['token'];
    if(token) {
        let signedInUser = store.state.signedInUser;
        signedInUser.token = token;
        store.commit("SET_SIGNED_IN_USER", signedInUser);
    }

    return response;
}