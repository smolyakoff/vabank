(function () {
    'use strict';

    var module = angular.module('vabank.ui');

    module.filter('linebreaks', function () {

        return function (input) {
            if (!_.isString(input)) {
                return input;
            }
            return input.replace(/(?:\r\n|\r|\n)/g, '<br />');
        };

    });
})();