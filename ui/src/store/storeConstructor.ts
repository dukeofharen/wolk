import {StateModel} from "@/models/store/stateModel";
import {StoreOptions} from "vuex";

const storeTypeEnum = {
    GETTER: 0,
    MUTATION: 1,
    ACTION: 2
};

const storeMap = [{
    type: storeTypeEnum.GETTER,
    content: require('@/store/getters/todotxt')
},{
    type: storeTypeEnum.GETTER,
    content: require('@/store/getters/users')
}, {
    type: storeTypeEnum.ACTION,
    content: require('@/store/actions/attachments')
}, {
    type: storeTypeEnum.ACTION,
    content: require('@/store/actions/backups')
}, {
    type: storeTypeEnum.ACTION,
    content: require('@/store/actions/notebooks')
}, {
    type: storeTypeEnum.ACTION,
    content: require('@/store/actions/notes')
}, {
    type: storeTypeEnum.ACTION,
    content: require('@/store/actions/users')
}, {
    type: storeTypeEnum.MUTATION,
    content: require('@/store/mutations/attachments')
}, {
    type: storeTypeEnum.MUTATION,
    content: require('@/store/mutations/general')
}, {
    type: storeTypeEnum.MUTATION,
    content: require('@/store/mutations/notebooks')
}, {
    type: storeTypeEnum.MUTATION,
    content: require('@/store/mutations/notes')
}, {
    type: storeTypeEnum.MUTATION,
    content: require('@/store/mutations/todotxt')
}, {
    type: storeTypeEnum.MUTATION,
    content: require('@/store/mutations/ui')
}, {
    type: storeTypeEnum.MUTATION,
    content: require('@/store/mutations/users')
}];

export function constructStore(state: StateModel): StoreOptions<StateModel> {
    let result: any = {
        state: state,
        mutations: {},
        actions: {},
        getters: {}
    };
    for (let storeResult of storeMap) {
        let typeKey = "";
        switch (storeResult.type) {
            case storeTypeEnum.GETTER:
                typeKey = 'getters';
                break;
            case storeTypeEnum.MUTATION:
                typeKey = 'mutations';
                break;
            case storeTypeEnum.ACTION:
                typeKey = 'actions';
                break;
        }
        
        for (let key in storeResult.content) {
            let value = storeResult.content[key];
            if (typeof value === 'function') {
                result[typeKey][key] = value;
            }
        }
    }
    return result;
};