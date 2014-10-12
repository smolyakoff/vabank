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
                controller: 'adminController',
                data: {
                    title: 'VaDmin - Панель управления',
                    access: {
                        allowAnonymous: false,
                        roles: ['Admin']
                    }
                }
            }).state('admin.scheduler', {
                url: '/scheduler',
                templateUrl: '/Client/app/areas/admin/scheduler/scheduler.html',
                data: {
                    title: 'VaDmin - Планировщик',
                }
                
            }).state('admin.systemLog', {
                url: '/logs/system',
                templateUrl: '/Client/app/areas/admin/system-log/system-log.html',
                data: {
                    title: 'VaDmin - Системный лог',
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
                
            }).state('admin.userManagement', {
                url: '/users',
                'abstract': true,
                template: '<div data-ui-view=""></div>',
                data: {
                    title: 'VaDmin - Управление пользователями',
                },                
            }).state('admin.userManagement.list', {
                url: '',
                templateUrl: '/Client/app/areas/admin/user-management/user-list.html',
                controller: 'userListController',
            }).state('admin.userManagement.editUser', {
                url: '/:id',
                templateUrl: '/Client/app/areas/admin/user-management/edit-user.html',
                controller: 'editUserController',
                resolve: {
                    data: ['$stateParams', 'routingResolve', 'userManagementService', function ($stateParams, routingResolve, userManager) {
                        var User = userManager.User;
                        if ($stateParams.id === 'add') {
                            return {user: User.defaults.new, profile: User.defaults.newProfile};
                        } else {
                            var params = { userId: $stateParams.id };
                            return routingResolve.resolveAll(
                                [User.get(params).$promise, User.getProfile(params).$promise],
                                ['user', 'profile']
                            );
                        }
                    }]
                }
            });;
    }

})();