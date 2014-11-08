(function () {
    'use strict';

    var module = angular.module('vabank.ui');

    module.filter('cardnumber', [function () {

        return function (input) {
            if (!_.isString(input)) {
                return input;
            }
            if (input.length < 16) {
                return input;
            }
            return input.substr(0, 4) + '-' +
                input.substr(4, 4) + '-' +
                input.substr(8, 4) + '-' +
                input.substr(12, 4);
        };

    }]);
})();