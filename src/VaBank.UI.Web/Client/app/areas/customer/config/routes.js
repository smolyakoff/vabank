(function () {
    'use strict';

    angular.module('vabank.webapp')
        .config(registerRoutes);

    registerRoutes.$inject = ['$stateProvider'];

    function registerRoutes($stateProvider) {
        $stateProvider
            .state('customer', {
                url: '/',
                'abstract': true,
                templateUrl: '/Client/app/areas/customer/customer.html',
                controller: 'customerController',
                data: {
                    title: 'VaBank - Мой кабинет',
                    subtitle: 'Мой кабинет',
                    access: {
                        allowAnonymous: false,
                        roles: ['Customer']
                    }
                },
            })
            .state('customer.cabinet', {
                url: '',
                templateUrl: '/Client/app/areas/customer/cabinet/cabinet.html',
            })
            .state('customer.profile', {
                url: 'profile',
                templateUrl: '/Client/app/areas/customer/profile/profile.html',
                data: {
                    title: 'VaBank - Профиль',
                    subtitle: 'Мой профиль'
                },
                controller: 'profileController'
            });
    }

})();