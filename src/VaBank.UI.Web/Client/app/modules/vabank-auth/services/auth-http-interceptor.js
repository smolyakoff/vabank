(function () {
    'use strict';

    angular
        .module('app')
        .factory('authHttpInterceptor', authHttpInterceptor);

    authHttpInterceptor.$inject = ['$http'];

    function authHttpInterceptor($http) {
    }
})();