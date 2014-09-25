(function () {
    'use strict';

    angular
        .module('vabank.ui')
        .factory('dataUtil', dataUtil);

    function dataUtil() {

        var FilterOperator = {
            Equal: 'eq',
            NotEqual: 'neq',
            GreaterThan: 'gt',
            LessThan: 'lt',
            In: 'in'
        };

        var operators = _.values(FilterOperator);

        var dumpValue = function(value) {
            if (_.isUndefined(value)) {
                value = null;
            }
            return JSON.stringify(value);
        };

        function Filter(options) {
            if (!Filter.schema(options)) {
                var errors = Filter.schema.errors(options);
                throw new TypeError('Invalid options passed!\n' + JSON.stringify(errors));
            }
            this.propertyName = options.propertyName;
            this.operator = options.operator;
            this.value = options.value;
        }
        Filter.schema = schema({
            propertyName: String,
            operator: operators,
        });
        Filter.prototype.toString = function () {
            return '(' + this.propertyName + ' ' + this.operator + ' ' + dumpValue(this.value) + ')';
        };
        Filter.prototype.toJSON = function() {
            return JSON.stringify({
                propertyName: this.propertyName,
                operator: this.operator,
                value: this.value,
                type: 'filter'
            });
        };

        var FilterLogic = {
            Or: 'or',
            And: 'and'
        };
        var logics = _.values(FilterLogic);


        var Combiner = (function () {
            var filters = [];
            function CombinerImpl(options) {
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
            CombinerImpl.prototype.toString = function () {
                var filterStrings = _.map(filters, function(x) {
                    return x.toString();
                });
                return '(' + filterStrings.join(' ' + this.logic + ' ') + ')';
            };
            CombinerImpl.prototype.toJSON = function () {
                JSON.stringify({
                    logic: this.logic,
                    filters: _.map(filters, function (x) { return x.toJSON(); }),
                    type: 'combiner'
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