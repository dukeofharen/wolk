var path = require('path')

module.exports = {
    lintOnSave: false,
    publicPath: '',
    devServer: {
        port: 8080,
        proxy: {
            '/api': {
                target: 'http://localhost:5010'
            }
        }
    }
}