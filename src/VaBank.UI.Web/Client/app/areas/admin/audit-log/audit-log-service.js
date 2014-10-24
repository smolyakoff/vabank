(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('auditLogService', auditLogService);

    auditLogService.$inject = ['$resource', 'dataUtil', 'userManagementService'];

    function auditLogService($resource, dataUtil, userManagementService) {

        var LogEntry = (function () {

            var defaults = {
                filter: function() {
                    return {
                        from: {
                            propertyName: 'timestampUtc',
                            operator: dataUtil.filters.operator.GreaterThanOrEqual,
                            value: moment().startOf('day').subtract(3, 'days'),
                        },
                        to: {
                            propertyName: 'timestampUtc',
                            operator: dataUtil.filters.operator.LessThan,
                            value: moment().startOf('day').add(1, 'day'),
                        },
                        userId: {
                            propertyName: 'operation.user.id',
                            operator: dataUtil.filters.operator.Equal,
                            propertyType: 'guid',
                            value: dataUtil.filters.markers.any('Любой')
                        },
                        code: {
                            propertyName: 'code',
                            operator: dataUtil.filters.operator.In,
                            value: []
                        }
                    };
                }
            };

            var LogEntryImpl = $resource('/api/logs/audit/:operationId', {}, {
                query: {
                    isArray: true,
                    params: { filter: dataUtil.filters.combine(defaults.filter(), dataUtil.filters.logic.And).toLINQ() }
                },
                lookup: { url: '/api/logs/audit/lookup' }
            });
            LogEntryImpl.defaults = defaults;

            return LogEntryImpl;

        })();

        return {            
            LogEntry: LogEntry,
            User: userManagementService.User
        };
    }
})();