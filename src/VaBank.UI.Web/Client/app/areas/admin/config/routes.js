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
                data: ['$q', 'systemLogService', function ($q, systemLogService) {
                    //TODO: make this func as util
                    var LogEntry = systemLogService.LogEntry;
                    return $q.all([LogEntry.query().$promise, LogEntry.lookup().$promise]).then(function (arr) {
                        return {
                            logs: arr[0],
                            lookup: arr[1]
                        };
                    });
                }]
            }
        });
    }
    
})();