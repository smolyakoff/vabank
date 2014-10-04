(function () {
    'use strict';

    angular.module('vabank.webapp')
           .config(registerRoutes);

    registerRoutes.$inject = ['$stateProvider', '$urlRouterProvider'];
     
    function registerRoutes($stateProvider, $urlRouterProvider) {
        $stateProvider.state('admin', {
            url: '/admin',
            templateUrl: '/Client/app/areas/admin/admin.html'
        }).state('admin.scheduler', {
            url: '/scheduler',
            templateUrl: '/Client/app/areas/admin/scheduler/scheduler.html'
        }).state('admin.systemLog', {
            url: '/logs/system',
            templateUrl: '/Client/app/areas/admin/system-log/system-log.html',
            controller: 'systemLogController',
            resolve: {
                data: ['routingResolve', 'systemLogService', function (routingResolve, systemLogService) {
                    var LogEntry = systemLogService.LogEntry;
                    return routingResolve.resolveAll(
                        [LogEntry.query().$promise, LogEntry.lookup().$promise],
                        ['logs', 'lookup']);
                }]
            }
        });
    }
    
})();