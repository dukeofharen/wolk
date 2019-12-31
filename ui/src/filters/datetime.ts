import Vue from 'vue'
import moment from 'moment'

Vue.filter('datetime', (value: string) => {
    let date = moment(value);
    return date.format('YYYY-MM-DD HH:mm:ss')
});

Vue.filter('date', (value: string) => {
    let date = moment(value);
    return date.format('YYYY-MM-DD')
});