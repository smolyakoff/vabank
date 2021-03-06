﻿(function() {
    'use strict';

    angular.module('vabank.webapp')
        .config(registerRoutes);

    registerRoutes.$inject = ['$stateProvider', '$urlRouterProvider'];

    function registerRoutes($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('admin', {
                url: '/admin',
                templateUrl: '/Client/app/areas/admin/admin.html',
                'abstract': true,
                controller: 'adminController',
                data: {
                    title: 'VaDmin - Панель управления',
                    access: {
                        allowAnonymous: false,
                        roles: ['Admin']
                    }
                }
            })
            .state('admin.dashboard', {
                url: '',
                templateUrl: '/Client/app/areas/admin/dashboard/dashboard.html',
                controller: 'dashboardController'
            })
            .state('admin.scheduler', {
                url: '/scheduler',
                templateUrl: '/Client/app/areas/admin/scheduler/scheduler.html',
                data: {
                    title: 'VaDmin - Планировщик'
                }
                
            }).state('admin.systemLog', {
                url: '/logs/system',
                templateUrl: '/Client/app/areas/admin/system-log/system-log.html',
                data: {
                    title: 'VaDmin - Системный лог'
                },
                controller: 'systemLogController',
                resolve: {
                    data: ['routingResolve', 'systemLogService', function(routingResolve, systemLogService) {
                        var LogEntry = systemLogService.LogEntry;
                        return routingResolve.resolveAll(
                            [LogEntry.query().$promise, LogEntry.lookup().$promise],
                            ['logs', 'lookup']);
                    }]
                }
                
            }).state('admin.auditLog', {
                url: '/logs/audit',
                templateUrl: '/Client/app/areas/admin/audit-log/audit-log.html',
                data: {
                    title: 'VaDmin - Аудит лог',
                },
                controller: 'auditLogController',
                resolve: {
                    data: ['routingResolve', 'auditLogService', function (routingResolve, auditLogService) {
                        var LogEntry = auditLogService.LogEntry;
                        return routingResolve.resolveAll(
                            [LogEntry.query().$promise, LogEntry.lookup().$promise],
                            ['logs', 'lookup']);
                    }]
                }
              
            }).state('admin.userManagement', {
                url: '/users',
                'abstract': true,
                template: '<div data-ui-view=""></div>',
                data: {
                    title: 'VaDmin - Управление пользователями'
                }
            }).state('admin.userManagement.list', {
                url: '',
                templateUrl: '/Client/app/areas/admin/user-management/user-list.html',
                controller: 'userListController'
            }).state('admin.userManagement.editUser', {
                url: '/:id',
                templateUrl: '/Client/app/areas/admin/user-management/edit-user.html',
                controller: 'editUserController',
                resolve: {
                    data: ['$stateParams', 'userManagementService', function ($stateParams, userManager) {
                        var User = userManager.User;
                        var Profile = userManager.Profile;
                        var PaymentProfile = userManager.PaymentProfile;
                        if ($stateParams.id === 'add') {
                            return {
                                user: User.defaults.new,
                                profile: Profile.defaults.new,
                                paymentProfile: PaymentProfile.defaults.new
                            };
                        } else {
                            var params = { userId: $stateParams.id };
                            return Profile.getFull(params).$promise;
                        }
                    }]
                }
            }).state('admin.cardManagement', {
                url: '/accounts',
                'abstract': true,
                template: '<div data-ui-view=""></div>',
                data: {
                    title: 'VaDmin - Карт-счета'
                },
            }).state('admin.cardManagement.list', {
                url: '',
                templateUrl: '/Client/app/areas/admin/card-management/card-account-list.html',
                controller: 'cardAccountListController',
                resolve: {
                    data: ['routingResolve', 'cardManagementService', function (routingResolve, cardManagementService) {
                        var CardAccount = cardManagementService.CardAccount;
                        return routingResolve.resolveAll(
                            [CardAccount.lookup().$promise],
                            ['lookup']);
                    }]
                }

            }).state('admin.cardManagement.newAccount', {
                url: '/new',
                templateUrl: '/Client/app/areas/admin/card-management/new-account.html',
                controller: 'newAccountController',
                resolve: {
                    data: ['routingResolve', 'cardManagementService', function (routingResolve, cardManagementService) {
                        var CardAccount = cardManagementService.CardAccount;
                        return routingResolve.resolveAll(
                            [CardAccount.lookup().$promise],
                            ['lookup']);
                    }]
                }
            }).state('admin.transactionLog', {
                url: '/log/transaction',
                templateUrl: '/Client/app/areas/admin/transaction-log/transaction-log.html',
                controller: 'transactionLogController',
                resolve: {
                    data: ['transactionLogService', function(transactionLogService) {
                        return transactionLogService.LogEntry.query().$promise;
                    }]
                }
            });
    }

})();