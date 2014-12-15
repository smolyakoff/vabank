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
                }
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
                }
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
                    data: ['myCardsService', function(myCardsService) {
                        var accounts = myCardsService.CardAccount.query();
                        return accounts;
                    }]
                }
            })
            .state('customer.cards.transfer', {
                url: '/transfer?from',
                templateUrl: '/Client/app/areas/customer/my-cards/transfer.html',
                controller: 'transferController',
                resolve: {
                    data: ['myCardsService', 'profileService', 'routingResolve',
                        function (myCardsService, profileService, routingResolve) {
                            var cards = myCardsService.Card.queryNotBlocked();
                            var profile = profileService.Profile.get().$promise;
                            var lookup = myCardsService.Transfer.lookup().$promise;
                            return routingResolve.resolveAll([cards, profile, lookup], ['cards', 'profile', 'lookup']);
                        }]
                }
            })
            .state('customer.payments', {
                url: 'payments',
                'abstract': true,
                templateUrl: '/Client/app/areas/customer/payments/payments.html',
                data: {
                    title: 'VaBank - Платежи',
                    subtitle: 'Оплата услуг'
                }
            })
            .state('customer.payments.payment', {
                url: '/pay',
                templateUrl: '/Client/app/areas/customer/payments/payment.html',
                controller: 'paymentController',
                resolve: {
                    data: ['$q', 'paymentService', 'profileService', function($q, paymentService, profileService) {
                        return $q.all({
                            cards: paymentService.Card.queryNotBlocked(),
                            profile: profileService.Profile.get().$promise
                        });
                    }]
                }
            })
            .state('customer.payments.archive', {
                url: '/archive',
                templateUrl: '/Client/app/areas/customer/payments/payments-archive.html',
                controller: 'paymentsArchiveController'
            });
    }

})();