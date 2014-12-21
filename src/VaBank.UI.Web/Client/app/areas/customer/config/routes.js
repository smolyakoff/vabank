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
                resolve: {
                    hasCards: ['myCardsService', function(myCardsService) {
                        var Card = myCardsService.Card;
                        return Card.query().$promise.then(function(cards) {
                            return cards.length > 0;
                        });
                    }]
                }
            })
            .state('customer.cabinet', {
                url: '',
                templateUrl: '/Client/app/areas/customer/cabinet/cabinet.html',
                controller: 'cabinetController',
                resolve: {
                    data: ['myCardsService', 'profileService', 'routingResolve',
                        function (myCardsService, profileService, routingResolve) {
                            var cards = myCardsService.Card.query().$promise;
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
                }
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
                    data: ['$q','myCardsService', 'profileService',
                        function ($q, myCardsService, profileService) {
                            var cards = myCardsService.Card.queryAllowed();
                            var profile = profileService.Profile.get().$promise;
                            var lookup = myCardsService.Transfer.lookup().$promise;
                            var deferred = $q.defer();
                            $q.all({ cards: cards, profile: profile, lookup: lookup }).then(function(result) {
                                if (result.cards.length === 0) {
                                    var error = new Error('Transition disabled');
                                    error.name = 'TransitionError';
                                    error.notification = {
                                        type: 'warning',
                                        label: 'Нет доступных карт',
                                        message: 'Нет разрешенных карт для проведения перевода'
                                    }
                                    error.redirectState = 'customer.cards.list';
                                    deferred.reject(error);
                                } else {
                                    deferred.resolve(result);
                                }
                            });
                            return deferred.promise;
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
                url: '/pay/:code?paymentId',
                templateUrl: '/Client/app/areas/customer/payments/payment.html',
                controller: 'paymentController',
                resolve: {
                    data: ['$q', '$stateParams', 'paymentService', 'profileService', function ($q, $stateParams, paymentService, profileService) {
                        var deferred = $q.defer();
                        $q.all({
                            cards: paymentService.Card.queryAllowed(),
                            profile: profileService.Profile.get().$promise,
                            prototype: $stateParams.paymentId 
                                ? paymentService.Payment.getPrototype({operationId: $stateParams.paymentId}).$promise
                                : null,
                            template: $stateParams.code 
                                ? paymentService.Payment.getTemplate({code: $stateParams.code}).$promise
                                : null
                        }).then(function(result) {
                            if (result.cards.length === 0) {
                                var error = new Error('Transition disabled');
                                error.name = 'TransitionError';
                                error.notification = {
                                    type: 'warning',
                                    label: 'Нет доступных карт',
                                    message: 'Нет разрешенных карт для совершения платежа'
                                }
                                error.redirectState = 'customer.payments.archive';
                                deferred.reject(error);
                            } else {
                                deferred.resolve(result);
                            }
                        });
                        return deferred.promise;
                    }]
                }
            })
            .state('customer.payments.archive', {
                url: '/archive',
                templateUrl: '/Client/app/areas/customer/payments/payments-archive.html',
                controller: 'paymentsArchiveController',
                resolve: {
                    data: ['paymentService', function(paymentService) {
                        return paymentService.Payment.query().$promise;
                    }]
                }
            });
    }

})();