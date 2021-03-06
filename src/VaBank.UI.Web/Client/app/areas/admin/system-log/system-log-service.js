﻿(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('systemLogService', systemLogService);

    systemLogService.$inject = ['$resource', 'dataUtil'];

    function systemLogService($resource, dataUtil) {

        var LogEntry = (function() {
            var defaults = {
                filter: function() {
                    return {
                        from: {
                            propertyName: 'timestampUtc',
                            operator: dataUtil.filters.operator.GreaterThanOrEqual,
                            value: moment().date(1).utc().startOf('day').toDate(),
                        },
                        to: {
                            propertyName: 'timestampUtc',
                            operator: dataUtil.filters.operator.LessThan,
                            value: moment().utc().startOf('day').add(1, 'd').toDate(),
                        },
                        type: {
                            propertyName: 'type',
                            operator: dataUtil.filters.operator.Equal,
                            value: dataUtil.filters.markers.any('Любой')
                        },
                        level: {
                            propertyName: 'level',
                            operator: dataUtil.filters.operator.In,
                            value: []
                        }
                    };
                }
            };

            var LogEntryImpl = $resource('/api/logs/system', {}, {
                query: {
                    isArray: true,
                    params: {
                        filter: dataUtil.filters.combine(defaults.filter(), dataUtil.filters.logic.And).toLINQ()
                    }
                },
                clear: {
                    url: '/api/logs/system/clear',
                    method: 'POST'
                },
                lookup: {
                    url: '/api/logs/system/lookup',
                    isArray: false
                },
                exception: {
                    url: '/api/logs/system/:id/exception',
                    isArray: false
                }
            });
            LogEntryImpl.defaults = defaults;
            return LogEntryImpl;

        })();

        return {            
          LogEntry: LogEntry  
        };
    }
})();