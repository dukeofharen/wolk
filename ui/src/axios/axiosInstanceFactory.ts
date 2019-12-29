import axios from 'axios';
import urls from '@/urls';
import handleError from '@/axios/errorInterceptor';
import handleJwtRenewal from '@/axios/renewTokenInterceptor';
import addJwtToRequest from '@/axios/addTokenToRequestInterceptor';

export default function createInstance() {
    let instance = axios.create({
        baseURL: urls.rootUrl,
        params: {}
    });
    instance.interceptors.response.use(
        response => response,
        error => handleError(error)
    );
    instance.interceptors.response.use(response => handleJwtRenewal(response));
    instance.interceptors.request.use(request => addJwtToRequest(request));
    return instance;
}