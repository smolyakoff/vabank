(function () {
    'use strict';

    angular.module('vabank.webapp')
           .config(registerRoutes);

    registerRoutes.$inject = ['$locationProvider', '$stateProvider', '$urlRouterProvider'];

    function registerRoutes($locationProvider, $stateProvider, $urlRouterProvider) {
        $locationProvider.html5Mode(true);

        $stateProvider.state('cabinet', {            
            url: '/',
            templateUrl: '/Client/app/areas/customer/cabinet.html'
            //controller: ['$state', function($state) {
            //    $state.go('login');
            //}]
        });

        $stateProvider.state('login', {            
            url: '/login',
            templateUrl: '/Client/app/areas/global/auth/login.html'
        });

        $stateProvider.state('error', {
            abstract: true,
            template: '<div data-ui-view=\"\"></div>'
        });
        $stateProvider.state('error.500', {
            url: '/error/500',
            templateUrl: '/Client/app/areas/global/errors/500.html'
        });
    }

})();