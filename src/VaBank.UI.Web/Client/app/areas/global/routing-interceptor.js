(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('routingInterceptor', routingInterceptor);

    routingInterceptor.$inject = ['$rootScope', 'cfpLoadingBar'];

    function routingInterceptor($rootScope, cfpLoadingBar) {

        var onStateChangeStart = function() {
            cfpLoadingBar.start();
        };

        var onStateChangeSuccess = function () {
            cfpLoadingBar.complete();
        };


        var initialize = function() {
            $rootScope.$on('$stateChangeStart', onStateChangeStart);
            $rootScope.$on('$stateChangeSuccess', onStateChangeSuccess);
        };

        return {
            initialize: initialize
        };
    }
})();