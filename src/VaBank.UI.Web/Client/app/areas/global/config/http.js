(function() {
    'use strict';

    angular.module('vabank.webapp').config(httpConfig);

    httpConfig.$inject = ['$httpProvider'];
    
    function httpConfig($httpProvider) {
        $httpProvider.interceptors.push('httpErrorNotifierInterceptor');
    }
    
})();