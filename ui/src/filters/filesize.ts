import Vue from 'vue'

Vue.filter('filesize', (value: number) => {
    if (value > 1000000) {
        return `${(value / 1000000).toFixed(2)}mb`;
    } else if (value > 1000) {
        return `${(value / 1000).toFixed(2)}kb`;
    }

    return `${value}b`;
});