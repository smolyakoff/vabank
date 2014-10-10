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

        var FilterPropertyType = {
            Auto: 'auto',
            Byte: 'byte',
            Short: 'short',
            Int: 'int',
            Long: 'long',
            Char: 'char',
            String: 'string',
            Float: 'float',
            Double: 'double',
            Decimal: 'decimal',
            DateTime: 'datetime',
            Guid: 'guid',
            Boolean: 'boolean'
        };
        
        var markers = {
            any: function Any(label, labelPropertyName) {
                if (!(this instanceof Any)) {
                    return new Any(label, labelPropertyName);
                }
                label = label || 'Any';
                labelPropertyName = labelPropertyName || 'label';
                this._labelPropertyName = labelPropertyName;
                this[labelPropertyName] = label;
            }
        };
        markers.any.prototype.toString = function() {
            return this[this._labelPropertyName];
        };

        var operators = _.values(FilterOperator);
        var filterPropertyTypes = _.values(FilterPropertyType);

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

            var isInFamilyOperator = function(operator) {
                return operator === FilterOperator.In || operator === FilterOperator.NotIn;
            };
            
            var isEqFamilyOperator = function (operator) {
                return operator === FilterOperator.Equal || operator === FilterOperator.NotEqual;
            };

            var isEmptyOrAnyMarkerArray = function(value) {
                if (!_.isArray(value)) {
                    return true;
                }
                if (value.length === 0) {
                    return true;
                }
                if (_.all(value, function(x) { return x instanceof markers.any; })) {
                    return true;
                }
                return false;
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

            var emptyQuery = function() {
                return '1 == 1';
            };

            var emptyFilter = {
                propertyName: '1',
                operator: FilterOperator.Equal,
                propertyType: 'int',
                value: '1',
                type: 'simple'
            };

            var functionOverParameterQuery = function(options, parameters) {
                var parameterName = addParameter(parameters, options.value);
                var prefix = options.isNegative ? '!' : '';
                return prefix + parameterName + '.' + options.functionName + '(' + options.propertyName + ')';
            };

            var eqQuery = function (options, parameters) {
                if (options.value instanceof markers.any) {
                    return emptyQuery();
                }
                return operatorQuery(options, parameters);
            };
            
            var inQuery = function (options, parameters) {
                if (isEmptyOrAnyMarkerArray(options.value)) {
                    return emptyQuery();
                }
                return functionOverParameterQuery(options, parameters);
            };

            function FilterImpl(options) {
                if (!FilterImpl.schema(options)) {
                    var errors = FilterImpl.schema.errors(options);
                    throw new TypeError('Invalid options passed!\n' + JSON.stringify(errors));
                }
                this.propertyType = angular.isString(options.propertyType)
                    ? options.propertyType
                    : FilterPropertyType.Auto;
                this.propertyName = options.propertyName;
                this.operator = options.operator;
                this.value = options.value;
            }
            FilterImpl.schema = schema({
                propertyName: String,
                propertyType: [null, filterPropertyTypes],
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
                        query = eqQuery({
                            propertyName: this.propertyName,
                            value: this.value,
                            operator: this.operator
                        }, parameters);
                        break;
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
                        query = inQuery({
                            propertyName: this.propertyName,
                            value: this.value,
                            functionName: functions[FilterOperator.In],
                            isNegative: false
                        }, parameters);
                        break;
                    case FilterOperator.NotIn:
                        query = inQuery({
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
                    q: query,
                    t: this.propertyType
                };
                return linq;
            };
            FilterImpl.prototype.toObject = function () {
                if (isInFamilyOperator(this.operator) && isEmptyOrAnyMarkerArray(this.value)) {
                    return angular.copy(emptyFilter);
                }
                if (isEqFamilyOperator(this.operator) && this.value instanceof markers.any) {
                    return angular.copy(emptyFilter);
                }
                return {
                    propertyName: this.propertyName,
                    propertyType: this.propertyType,
                    operator: this.operator,
                    value: this.value,
                    type: 'simple'
                };
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
                var filterLinqs = _.map(filters, function (x) {
                    var linq = x.toLINQ(parameters);
                    return linq;
                });
                var filterStrings = _.pluck(filterLinqs, 'q');
                var filterTypes = _.pluck(filterLinqs, 't');
                var linq = {                    
                    q: '(' + filterStrings.join(' ' + this.logic + ' ') + ')',
                    p: JSON.stringify(parameters),
                    t: filterTypes.join(', ')
                };
                return linq;
            };
            CombinerImpl.prototype.toObject = function () {
                return {
                    logic: this.logic,
                    filters: _.map(filters, function(x) { return x.toObject(); }),
                    type: 'combined'
                };
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
        
        var SortDirection = {
            Asc: 'asc',
            Desc: 'desc'
        };
        var sortDirections = _.values(SortDirection);


        var Sort = (function () {
            function SortImpl(options) {
                if (!SortImpl.schema(options)) {
                    var errors = SortImpl.schema.errors(options);
                    throw new TypeError('Invalid options passed!\n' + JSON.stringify(errors));
                }
                this.propertyName = options.propertyName;
                this.direction = options.direction;
            };
            SortImpl.schema = schema({                
                propertyName: String,
                direction: sortDirections 
            });
            SortImpl.prototype.toLINQ = function() {
                return this.propertyName + ' ' + this.direction;
            };
            SortImpl.prototype.toObject = function() {
                return {                    
                    propertyName: this.propertyName,
                    direction: this.direction
                };
            };
            return SortImpl;
        })();

        var MultiSort = (function () {
            var sorts = [];
            function MultiSortImpl() {
                sorts = [];
            }
            MultiSortImpl.prototype.add = function (sort) {
                if (!Sort.schema(sort)) {
                    throw new TypeError("Invalid sort passed!");
                }
                if (sort instanceof Sort && sort.isEmpty()) {
                    return;
                }
                if (!(sort instanceof Filter)) {
                    sort = new Sort(sort);
                }
                sorts.push(sort);
            };
            MultiSortImpl.prototype.isEmpty = function () {
                return sorts.length == 0;
            };
            MultiSortImpl.prototype.toLINQ = function() {
                var linqs = _.map(sorts, function (x) {
                    return x.toLINQ();
                });
                return linqs.join(', ') || null;
            };
            MultiSortImpl.prototype.toObject = function () {
                if (this.isEmpty()) {
                    return null;
                }
                if (sorts.length === 1) {
                    return sorts[0].toObject();
                }
                return {                    
                  sorts: _.map(sorts, function(x) {
                      return x.toObject();
                  })  
                };
            };
            return MultiSortImpl;
        })();

        var combineSorts = function(sorts) {
            var schemaCheck = function (x) {
                return Sort.schema(x);
            };
            if (!_.all(sorts, schemaCheck)) {
                throw new TypeError('Invalid sorts passed.');
            }
            var multiSort = new MultiSort();
            _.forEach(sorts, function (x) {
                multiSort.add(x);
            });
            return multiSort;
        };

        var toPageNumberAndSize = function (paging) {
            var pageNumber = (paging.start / paging.number) + 1;
            var pageSize = paging.number;
            return {
                pageNumber: pageNumber,
                pageSize: pageSize
            };
        };

        var queryFromStTable = function(tableState) {
            var params = {};
            if (tableState.sort.predicate) {
                params.sort = combineSorts([
                    {
                        propertyName: tableState.sort.predicate,
                        direction: tableState.sort.reverse ? 'desc' : 'asc'
                    }
                ]).toLINQ();
            }
            if (!_.isUndefined(tableState.pagination.number)) {
                angular.extend(params, toPageNumberAndSize(tableState.pagination));
            }
            return params;
        };

        return {
            query: {
              fromStTable: queryFromStTable  
            },
            filters: {
                operator: FilterOperator,
                logic: FilterLogic,
                combine: combineFilters,
                markers: markers
            },
            sort: {
                direction: SortDirection,
                combine: combineSorts
            },
            paging: {
                toPageNumberAndSize: toPageNumberAndSize
            }
        };
    }
})();