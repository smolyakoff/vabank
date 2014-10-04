(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('routingInterceptor', routingInterceptor);

    routingInterceptor.$inject = ['$rootScope','$filter', '$state', 'cfpLoadingBar', 'notificationService'];

    function routingInterceptor($rootScope, $filter, $state, cfpLoadingBar, notificationService) {

        var onStateChangeStart = function () {
            $rootScope.stateChanging = true;
            cfpLoadingBar.start();
        };

        var onStateChangeSuccess = function (event, toState, toParams, fromState, fromParams) {
            $rootScope.stateChanging = false;
            cfpLoadingBar.complete();
            var notifications = notificationService.pop(toState.name);
            var data = _.isObject(toState.data) ? toState.data : {};
            if (!data.disableNotifications) {
                _.forEach(notifications, function (x) {
                    notificationService.notify(x);
                });
            }
        };

        var onStateChangeError = function (event, toState, toParams, fromState, fromParams, error) {
            $rootScope.stateChanging = false;
            cfpLoadingBar.complete();
            var notification =  {
                state: 'error.500',
                type: 'error',
                title: 'DEBUG',
                message: '<pre>' + $filter('json')(error) + '</pre>'
            };
            notificationService.notify(notification);
            $state.go('error.500');
        };

        var onStateNotFound = function (event, unfoundState, fromState, fromParams) {
            $rootScope.stateChanging = false;
            cfpLoadingBar.complete();
            $state.go('error.500');
        };
      

        var initialize = function() {
            $rootScope.$on('$stateChangeStart', onStateChangeStart);
            $rootScope.$on('$stateChangeSuccess', onStateChangeSuccess);
            $rootScope.$on('$stateChangeError', onStateChangeError);
            $rootScope.$on('$stateNotFound', onStateNotFound);
        };

        return {
            initialize: initialize
        };
    }
})();