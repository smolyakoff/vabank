(function () {
    'use strict';

    angular.module('vabank.webapp')
           .config(registerRoutes);

    registerRoutes.$inject = ['$locationProvider', '$stateProvider', '$urlRouterProvider', 'serverInfo'];

    function registerRoutes($locationProvider, $stateProvider, $urlRouterProvider, serverInfo) {
        $locationProvider.html5Mode(true);

        $urlRouterProvider.otherwise('/');

        $stateProvider.state('login', {            
            url: '/login?redirect',
            templateUrl: '/Client/app/areas/global/auth/login.html',
            controller: 'loginController',
            data: {
                title: 'VaBank - Вход в систему',
                access: {
                    allowAnonymous: true
                }
            }
        });

        $stateProvider.state('error', {
            'abstract': true,
            template: '<div data-ui-view=\"\"></div>',
            data: {
                access: {
                   allowAnonymous: true
                }
            }
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