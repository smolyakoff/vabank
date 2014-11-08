(function () {
    'use strict';

    var module = angular.module('vabank.ui');

    module.filter('expdate', [function () {

        return function (input) {
            if (!_.isString(input)) {
                return input;
            }
            if (input.length < 4) {
                return input;
            }
            return input.substr(0, 2) + '.' + input.substr(2, 2);
        };

    }]);
})();