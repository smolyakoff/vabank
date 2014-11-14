(function () {
    'use strict';

    var module = angular.module('vabank.ui');

    module.filter('extcurrency', ['$locale', function($locale) {

        return function (input, symbol, precision) {
            if (_.isNull(input) || _.isUndefined(input)) {
                return input;
            }
            if (_.isUndefined(symbol)) {
                symbol = $locale.NUMBER_FORMATS.CURRENCY_SYM;
            }
            if (_.isUndefined(precision)) {
                precision = $locale.NUMBER_FORMATS.PATTERNS[1].maxFrac;
            }
            return accounting.formatMoney(input, symbol, precision);
        };

    }]);
})();