(function () {
    'use strict';

    angular.module('vabank.webapp')
           .config(registerRoutes);

    registerRoutes.$inject = ['$locationProvider', '$stateProvider', '$urlRouterProvider', 'serverInfo'];

    function registerRoutes($locationProvider, $stateProvider, $urlRouterProvider, serverInfo) {
        $locationProvider.html5Mode(true);

        $urlRouterProvider.otherwise('/');

        $stateProvider.state('login', {            
            url: '/login',
            templateUrl: '/Client/app/areas/global/auth/login.html',
            data: {
                title: 'VaBank - Вход в систему'
            }
        });

        $stateProvider.state('error', {
            'abstract': true,
            template: '<div data-ui-view=\"\"></div>'
        });
        
        $stateProvider.state('error.500', {
            url: '/error/500',
            templateUrl: '/Client/app/areas/global/errors/500.html',
            data: {
                title: 'VaBank - Ой-ой-ой',
                disableNotifications: !serverInfo.isDebug
            }
        });
    }

})();