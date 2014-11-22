(function() {
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
                controller: 'cabinetController',
                resolve: {
                    data: ['myCardsService', 'profileService', 'routingResolve',
                        function (myCardsService, profileService, routingResolve) {
                            var cards = myCardsService.Card.query.$promise;
                            var profile = profileService.Profile.get().$promise;
                            return routingResolve.resolveAll([cards, profile], ['cards', 'profile']);
                        }]
                }
            })
            .state('customer.profile', {
                url: 'profile',
                'abstract': true,
                templateUrl: '/Client/app/areas/customer/profile/profile.html',
                data: {
                    title: 'VaBank - Профиль',
                    subtitle: 'Мой профиль'
                },
            })
            .state('customer.profile.edit', {
                url: '',
                templateUrl: '/Client/app/areas/customer/profile/profile-edit.html',
                controller: 'profileEditController',
                resolve: {
                    data: ['profileService', function(profileService) {
                        return profileService.Profile.get().$promise;
                    }]
                }
            })
            .state('customer.profile.changePassword', {
                url: '/change-password',
                templateUrl: '/Client/app/areas/customer/profile/profile-change-pwd.html',
                controller: 'changePasswordController'
            })
            .state('customer.cards', {
                url: 'cards',
                'abstract': true,
                templateUrl: '/Client/app/areas/customer/my-cards/my-cards.html',
                data: {
                    title: 'VaBank - Платежные карты',
                    subtitle: 'Мои платежные карты'
                },
            })
            .state('customer.cards.list', {
                url: '',
                templateUrl: '/Client/app/areas/customer/my-cards/my-cards-list.html',
                controller: 'myCardsListController',
                resolve: {
                    data: ['myCardsService', function(myCardsService) {
                        return myCardsService.Card.query().$promise;
                    }]
                }
            })
            .state('customer.cards.accountStatement', {
                url: '/statement',
                templateUrl: '/Client/app/areas/customer/my-cards/account-statement.html',
                controller: 'accountStatementController',
                resolve: {
                    data: ['myCardsService', 'profileService', 'routingResolve',
                        function (myCardsService, profileService, routingResolve) {
                            var cards = myCardsService.Card.query().$promise;
                            var profile = profileService.Profile.get().$promise;
                            return routingResolve.resolveAll([cards, profile], ['cards', 'profile']);
                        }]
                }
            })
            .state('customer.cards.transfer', {
                url: '/transfer',
                templateUrl: '/Client/app/areas/customer/my-cards/transfer.html',
                controller: 'transferController',
                resolve: {
                    data: ['myCardsService', 'profileService', 'routingResolve',
                        function (myCardsService, profileService, routingResolve) {
                            var cards = myCardsService.Card.queryNotBlocked();
                            var profile = profileService.Profile.get().$promise;
                            return routingResolve.resolveAll([cards, profile], ['cards', 'profile']);
                        }]
                }
            });
    }

})();