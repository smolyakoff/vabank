(function () {
    'use strict';

    angular.module('vabank.webapp')
           .config(registerRoutes);

    registerRoutes.$inject = ['$locationProvider', '$stateProvider', '$urlRouterProvider'];

    function registerRoutes($locationProvider, $stateProvider, $urlRouterProvider) {
        $locationProvider.html5Mode(true);

        $stateProvider.state('home', {            
            url: '/',
            controller: ['$state', function($state) {
                $state.go('login');
            }]
        });

        $stateProvider.state('login', {            
            url: '/login',
            templateUrl: '/Client/app/auth/login.html'
        });
    }

})();