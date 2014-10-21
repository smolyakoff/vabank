(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('auditLogService', auditLogService);

    auditLogService.$inject = ['$resource', 'dataUtil', 'userManagementService'];

    function auditLogService($resource, dataUtil, userManagementService) {

        var defaults = {
            filter: {
                from: {
                    propertyName: 'timestampUtc',
                    operator: dataUtil.filters.operator.GreaterThanOrEqual,
                    value: moment().utc().startOf('day').subtract(3, 'days').toDate(),
                },
                to: {
                    propertyName: 'timestampUtc',
                    operator: dataUtil.filters.operator.LessThan,
                    value: moment().utc().startOf('day').add(1, 'day').toDate(),
                },
                userId: {
                    propertyName: 'userId',
                    operator: dataUtil.filters.operator.Equal,
                    value: dataUtil.filters.markers.any('Любой')
                },
                code: {
                    propertyName: 'code',
                    operator: dataUtil.filters.operator.In,
                    value: []
                }
            }
        };

        var LogEntry = $resource('/api/logs/audit', {}, {
            
        });
        
        LogEntry.defaults = defaults;

        return {            
            LogEntry: LogEntry,
            User: userManagementService.User
        };
    }
})();