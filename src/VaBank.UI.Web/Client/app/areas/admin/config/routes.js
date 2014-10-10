(function() {
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
                templateUrl: '/Client/app/areas/admin/user-management/user-management.html',
                data: {
                    title: 'VaDmin - Управление пользователями',
                },                
                controller: 'userManagementController'
            });
    }

})();