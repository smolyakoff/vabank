(function () {
    'use strict';

    angular
        .module('vabank.ui')
        .service('controlUtil', controlUtil);

    function controlUtil() {

        var multiselect = (function () {
            
            var multiselectDefaults = {
                title: null,
                value: null,
                displayPropertyName: 'label',
                tickedPropertyName: 'ticked',
                preItems: [],
                tickMode: 'none',
                tickedValues: [],
            };

            var getTitle = function (x, options) {
                if (_.isNull(options.title) || _.isUndefined(options.title)) {
                    return x;
                }
                if (_.isFunction(options.title)) {
                    return options.title(x);
                }
                return x[options.title];
            };

            var getValue = function (x, options) {
                if (_.isNull(options.value) || _.isUndefined(options.value)) {
                    return x;
                }
                if (_.isFunction(options.value)) {
                    return options.value(x);
                }
                return x[options.value];
            };


            var getSelectChoices = function(arr, options) {
                if (!_.isArray(arr)) {
                    throw new TypeError("Should be an array!");
                }
                options = angular.extend({}, multiselectDefaults, options);

                var selectChoices = _.map(options.preItems.concat(arr), function (x) {
                    var o = {};
                    o[options.displayPropertyName] = getTitle(x, options);
                    o.value = getValue(x, options);
                    o[options.tickedPropertyName] = false;
                    return o;
                });

                if (options.tickMode === 'first' && selectChoices.length > 0) {
                    selectChoices[0][options.tickedPropertyName] = true;
                } else if (options.tickMode === 'all') {
                    _.forEach(selectChoices, function (x) {
                        x[options.tickedPropertyName] = true;
                    });
                } else if (options.tickMode === 'custom') {
                    _.forEach(selectChoices, function (x) {
                        if (_.contains(options.tickedValues, x)) {
                            x[options.tickedPropertyName] = true;
                        }
                    });
                }
                return selectChoices;
            };

            var getSelectedItems = function(choices, options) {
                if (!_.isArray(choices)) {
                    throw new TypeError("Should be an array!");
                }
                options = angular.extend({}, multiselectDefaults, options);
                var search = {};
                search[options.tickedPropertyName] = true;
                return _.chain(choices).where(search).pluck('value').value();
            };

            var getSingleItem = function(choices, array) {
                var selected = getSelectedItems(choices, array);
                if (selected.length > 0) {
                    return selected[0];
                }
                return undefined;
            };

            return {                
                getSelectChoices: getSelectChoices,
                getSelectedItems: getSelectedItems,
                getSingleItem: getSingleItem
            };

        })();


        var form = (function () {

            var create = function (template) {
                if (!_.isObject(template)) {
                    return {};
                }
                return _.object(_.map(template, function(v, k) {
                    return [k, v.value];
                }));
            };

            return {                
                create: create
            };
        })();


        return {
            multiselect: multiselect,
            form: form
        };
    }
})();