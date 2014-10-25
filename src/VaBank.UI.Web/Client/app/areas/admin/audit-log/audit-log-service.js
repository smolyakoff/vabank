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
                },
                filterValues: function() {
                    var filter = defaults.filter();
                    var keys = _.keys(filter);
                    var values = {};
                    _.forEach(keys, function(k) {
                        values[k] = filter[k].value;
                    });
                    return values;
                }
            };

            var LogEntryImpl = $resource('/api/logs/audit/:operationId', {}, {
                query: {
                    isArray: true,
                    params: { filter: dataUtil.filters.combine(defaults.filter(), dataUtil.filters.logic.And).toLINQ() }
                },
                lookup: { url: '/api/logs/audit/lookup' }
            });
            var queryResource = LogEntryImpl.query;
            LogEntryImpl.query = function (params) {
                var filter = defaults.filter();
                var values = angular.extend({}, defaults.filterValues(), params);
                filter.userId.propertyName = _.isNull(values.userId)
                    ? 'operation.user'
                    : 'operation.user.id';
                _.each(values, function(v, k) {
                    filter[k].value = v;
                });
                var linq = dataUtil.filters.combine(filter, dataUtil.filters.logic.And).toLINQ();
                return queryResource({filter: linq});
            };

            LogEntryImpl.defaults = defaults;

            return LogEntryImpl;

        })();

        return {            
            LogEntry: LogEntry,
            User: userManagementService.User
        };
    }
})();