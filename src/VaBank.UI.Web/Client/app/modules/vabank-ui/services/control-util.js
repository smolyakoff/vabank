(function () {
    'use strict';

    angular
        .module('vabank.ui')
        .service('controlUtil', controlUtil);

    function controlUtil() {

        var multiselectDefaults = {            
            displayPropertyName: 'label',
            tickedPropertyName: 'ticked',
            tickMode: 'none',
            tickedItems: []
        };

        var multiselect = {            
            getSelectChoices: function (arr, options) {
                if (!_.isArray(arr)) {
                    throw new TypeError("Should be an array!");
                }
                options = angular.extend({}, multiselectDefaults, options);
                var selectChoices = _.map(arr, function(x) {
                    var o = {};
                    o[options.displayPropertyName] = x;
                    o[options.tickedPropertyName] = false;
                    return o;
                });
                
                if (options.tickMode === 'first' && selectChoices.length > 0) {
                    selectChoices[0][options.tickedPropertyName] = true;
                } else if (options.tickMode === 'all') {
                    _.forEach(selectChoices, function(x) {
                        x[options.tickedPropertyName] = true;
                    });
                } else if (options.tickMode === 'custom') {
                    _.forEach(selectChoices, function (x) {
                        if (_.contains(options.tickedItems, x)) {
                            x[options.tickedPropertyName] = true;
                        }
                    });
                }
                return selectChoices;
            }
        };


        return {
            multiselect: multiselect
        };
    }
})();