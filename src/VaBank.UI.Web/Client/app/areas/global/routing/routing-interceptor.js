(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('routingInterceptor', routingInterceptor);

    routingInterceptor.$inject = ['$rootScope','$filter', '$state', 'cfpLoadingBar', 'notificationService', 'authService'];

    function routingInterceptor($rootScope, $filter, $state, cfpLoadingBar, notificationService, authService) {

        var onStateChangeStart = function (event, toState, toParams, fromState, fromParams) {
            debugger;
            $rootScope.stateChanging = true;
            var data = toState.data;
            var user = authService.getUser();
            if (data.access.allowAnonymous) {
                cfpLoadingBar.start();
            } else {
                var roles = data.access.roles;
                var hasAccess = _.all(roles, function(x) {
                    return user.isInRole(x);
                });
                if (hasAccess) {
                    cfpLoadingBar.start();
                } else {
                    event.preventDefault();
                    $state.go('login', {redirect: toState.name});
                }
            }
            
        };

        var onStateChangeSuccess = function (event, toState, toParams, fromState, fromParams) {
            var data = _.isObject(toState.data) ? toState.data : {};
            $rootScope.stateChanging = false;
            $rootScope.title = data.title || 'VaBank - Онлайн';
            cfpLoadingBar.complete();
            var notifications = notificationService.pop(toState.name);
            
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