(function () {
    'use strict';

    var module = angular.module('vabank.ui');

    module.filter('localdate', [function () {

        return function (input) {
            if (_.isString(input) || _.isDate(input)) {
                return moment.utc(input).local();
            }
            return input;
        };

    }]);
})();