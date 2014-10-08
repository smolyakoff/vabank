(function () {
    'use strict';

    angular.module('vabank.webapp')
        .config(registerRoutes);

    registerRoutes.$inject = ['$stateProvider'];

    function registerRoutes($stateProvider) {
        $stateProvider
            .state('cabinet', {
                url: '/',
                'abstract': true,
                templateUrl: '/Client/app/areas/customer/cabinet.html',
                controller: 'cabinetController',
                data: {
                    title: 'VaBank - Мой кабинет',
                    subtitle: 'Мой кабинет',
                    access: {
                        allowAnonymous: false,
                        roles: ['Customer']
                    }
                },
            })
            .state('cabinet.dashboard', {
                url: '',
                templateUrl: '/Client/app/areas/customer/dashboard/dashboard.html',
            })
            .state('cabinet.profile', {
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