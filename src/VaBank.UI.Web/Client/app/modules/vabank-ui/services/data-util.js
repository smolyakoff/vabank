(function () {
    'use strict';

    angular
        .module('vabank.ui')
        .factory('dataUtil', dataUtil);

    function dataUtil() {

        var FilterOperator = {
            Equal: '==',
            NotEqual: '!=',
            GreaterThan: '>',
            GreaterThanOrEqual: '>=',
            LessThan: '<',
            LessThanOrEqual: '<=',
            In: 'in',
            NotIn: '!in',
            StartsWith: 'startswith',
            NotStartsWith: '!startswith',
            EndsWith: 'endswith',
            NotEndsWith: '!endswith',
            Contains: 'contains',
            NotContains: '!contains'
        };

        var operators = _.values(FilterOperator);

        var Filter = (function () {

            var functions = {};
            functions[FilterOperator.StartsWith] = "StartsWith";
            functions[FilterOperator.EndsWith] = "EndsWith";
            functions[FilterOperator.Contains] = "Contains";
            functions[FilterOperator.In] = "Contains";

            var addParameter = function(parameters, value) {
                if (_.isUndefined(value)) {
                    value = null;
                }
                parameters.push(value);
                return '@' + (parameters.length - 1);
            };

            var operatorQuery = function(options, parameters) {
                var parameterName = addParameter(parameters, options.value);
                return options.propertyName + ' ' + options.operator + ' ' + parameterName;
            };

            var functionQuery = function(options, parameters) {
                var parameterName = addParameter(parameters, options.value);
                var prefix = options.isNegative ? '!' : '';
                return prefix + options.propertyName + '.' + options.functionName + '(' + parameterName + ')';
            };

            var functionOverParameterQuery = function(options, parameters) {
                var parameterName = addParameter(parameters, options.value);
                var prefix = options.isNegative ? '!' : '';
                return prefix + parameterName + '.' + options.functionName + '(' + options.propertyName + ')';
            };

            function FilterImpl(options) {
                if (!FilterImpl.schema(options)) {
                    var errors = FilterImpl.schema.errors(options);
                    throw new TypeError('Invalid options passed!\n' + JSON.stringify(errors));
                }
                this.propertyName = options.propertyName;
                this.operator = options.operator;
                this.value = options.value;
            }
            FilterImpl.schema = schema({
                propertyName: String,
                operator: operators,
            });
            FilterImpl.prototype.toLINQ = function (parameters) {
                if (_.isUndefined(parameters) || _.isNull(parameters)) {
                    parameters = [];
                }
                var query;
                switch (this.operator) {
                    case FilterOperator.Equal:
                    case FilterOperator.NotEqual:
                    case FilterOperator.GreaterThan:
                    case FilterOperator.GreaterThanOrEqual:
                    case FilterOperator.LessThan:
                    case FilterOperator.LessThanOrEqual:
                        query = operatorQuery({
                            propertyName: this.propertyName,
                            value: this.value,
                            operator: this.operator
                        }, parameters);
                        break;
                    case FilterOperator.In:
                        query = functionOverParameterQuery({
                            propertyName: this.propertyName,
                            value: this.value,
                            functionName: functions[FilterOperator.In],
                            isNegative: false
                        }, parameters);
                        break;
                    case FilterOperator.NotIn:
                        query = functionOverParameterQuery({
                            propertyName: this.propertyName,
                            value: this.value,
                            functionName: functions[FilterOperator.In],
                            isNegative: true
                        }, parameters);
                        break;
                    case FilterOperator.StartsWith:
                        query = functionQuery({                            
                            propertyName: this.propertyName,
                            value: this.value,
                            functionName: functions[FilterOperator.StartsWith],
                            isNegative: false
                        }, parameters);
                        break;
                    case FilterOperator.NotStartsWith:
                        query = functionQuery({                            
                            propertyName: this.propertyName,
                            value: this.value,
                            functionName: functions[FilterOperator.StartsWith],
                            isNegative: true
                        }, parameters);
                        break;
                    case FilterOperator.EndsWith:
                        query = functionQuery({                            
                            propertyName: this.propertyName,
                            value: this.value,
                            functionName: functions[FilterOperator.EndsWith],
                            isNegative: false
                        }, parameters);
                        break;
                    case FilterOperator.NotEndsWith:
                        query = functionQuery({                            
                            propertyName: this.propertyName,
                            value: this.value,
                            functionName: functions[FilterOperator.EndsWith],
                            isNegative: true
                        }, parameters);
                        break;
                    case FilterOperator.Contains:
                        query = functionQuery({                            
                            propertyName: this.propertyName,
                            value: this.value,
                            functionName: functions[FilterOperator.Contains],
                            isNegative: false
                        }, parameters);
                        break;
                    case FilterOperator.NotContains:
                        query = functionQuery({                            
                            propertyName: this.propertyName,
                            value: this.value,
                            functionName: functions[FilterOperator.Contains],
                            isNegative: true
                        }, parameters);
                        break;
                    default:
                        var message = 'Operator ' + this.operator + ' is not supported';
                        throw new Error(message, "Invalid operator");
                }
                var linq = {                    
                    p: JSON.stringify(parameters),
                    q: query
                };
                return linq;
            };
            FilterImpl.prototype.toJSON = function () {
                return JSON.stringify({
                    propertyName: this.propertyName,
                    operator: this.operator,
                    value: this.value,
                    type: 'simple'
                });
            };
            return FilterImpl;
        })();

        

        var FilterLogic = {
            Or: 'or',
            And: 'and'
        };
        var logics = _.values(FilterLogic);


        var Combiner = (function () {
            var filters = [];
            function CombinerImpl(options) {
                filters = [];
                if (!CombinerImpl.schema(options)) {
                    var errors = CombinerImpl.schema.errors(options);
                    throw new TypeError('Invalid options passed!\n' + JSON.stringify(errors));
                }
                this.logic = options.logic;
            }
            CombinerImpl.schema = schema({
                logic: logics
            });
            CombinerImpl.prototype.add = function (filter) {
                if (!(filter instanceof Combiner) && !Filter.schema(filter)) {
                    throw new TypeError("Invalid filter passed!");
                }
                if (filter instanceof Combiner && filter.isEmpty()) {
                    return;
                }
                if (Filter.schema(filter) && !(filter instanceof Filter)) {
                    filter = new Filter(filter);
                }
                filters.push(filter);
            };
            CombinerImpl.prototype.isEmpty = function() {
                return filters.length == 0;
            };
            CombinerImpl.prototype.getFilters = function () {
                return _.clone(filters);
            };
            CombinerImpl.prototype.toLINQ = function () {
                var parameters = [];
                var filterStrings = _.map(filters, function (x) {
                    var linq = x.toLINQ(parameters);
                    return linq.q;
                });
                var linq = {                    
                    q: '(' + filterStrings.join(' ' + this.logic + ' ') + ')',
                    p: JSON.stringify(parameters)
                };
                return linq;
            };
            CombinerImpl.prototype.toJSON = function () {
                JSON.stringify({
                    logic: this.logic,
                    filters: _.map(filters, function (x) { return x.toJSON(); }),
                    type: 'combined'
                });
            };
            return CombinerImpl;
        })();
        

        var combineFilters = function (filters, logic) {
            var options = {
                logic: logic
            };
            if (!Combiner.schema(options)) {
                throw new TypeError('Logic ' + logic + ' is not supported.');
            }
            var schemaCheck = function(x) {
                return Filter.schema(x) || (x instanceof Combiner);
            };
            if (!_.all(filters, schemaCheck)) {
                throw new TypeError('Invalid filters passed.');
            }
            var combiner = new Combiner(options);
            _.forEach(filters, function(x) {
                combiner.add(x);
            });
            return combiner;
        };

        return {
            filters: {
                operator: FilterOperator,
                logic: FilterLogic,
                combine: combineFilters
            }
        };
    }
})();