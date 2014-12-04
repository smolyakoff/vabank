(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('transactionLogService', transactionLogService);

    transactionLogService.$inject = ['$resource', 'dataUtil', 'cardManagementService', 'exchangeRateService'];

    function transactionLogService($resource, dataUtil, cardManagementService, exchangeRateService) {

        var LogEntry = (function () {

            var defaults = {
                filter: function() {
                    return {
                        from: {
                            propertyName: 'createdDateUtc',
                            operator: dataUtil.filters.operator.GreaterThanOrEqual,
                            value: moment().startOf('day').subtract(1, 'days'),
                            propertyType: 'datetime'
                        },
                        to: {
                            propertyName: 'createdDateUtc',
                            operator: dataUtil.filters.operator.LessThan,
                            value: moment().startOf('day').add(1, 'day'),
                            propertyType: 'datetime'
                        },
                        accountNo: {
                            propertyName: 'accountNo',
                            operator: dataUtil.filters.operator.In,
                            value: dataUtil.filters.markers.any('Любой'),
                            propertyType: 'string'
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

            var LogEntryImpl = $resource('/api/logs/transaction/:transactionId', {}, {
                query: {
                    isArray: true,
                    params: { filter: dataUtil.filters.combine(defaults.filter(), dataUtil.filters.logic.And).toLINQ() }
                }
            });
            var queryResource = LogEntryImpl.query;
            LogEntryImpl.defaults = defaults;
            LogEntryImpl.statuses = function() {
                return ["Failed", "Pending", "Completed"];
            };
            LogEntryImpl.query = function(params) {
                var filter = defaults.filter();
                params = params || {};
                var values = angular.extend({}, defaults.filterValues(), params);
                delete values.status;
                _.each(values, function (v, k) {
                    filter[k].value = v;
                });
                var linq = dataUtil.filters.combine(filter, dataUtil.filters.logic.And).toLINQ();
                return queryResource({ filter: linq, status: params.status });
            };

            return LogEntryImpl;

        })();

        return {            
            LogEntry: LogEntry,
            CardAccount: cardManagementService.CardAccount,
            Currency: exchangeRateService.Currency
        };
    }
})();