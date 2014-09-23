(function () {
    'use strict';

    angular.module('vabank.webapp')
           .config(registerRoutes);

    registerRoutes.$inject = ['$stateProvider', '$urlRouterProvider'];
    
    function registerRoutes($stateProvider, $urlRouterProvider) {
        $stateProvider.state('admin', {
            url: '/admin',
            templateUrl: '/Client/app/areas/admin/admin.html'
        });
    }
    
})();