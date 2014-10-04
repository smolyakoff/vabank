(function () {
    'use strict';

    var module = angular.module('vabank.ui');

    module.filter('truncate', ['$filter', function($filter) {

        return function(input, maxLength, continuation) {
            if (!_.isString(input)) {
                return input;
            }
            if (_.isUndefined(continuation)) {
                continuation = '...';
            }
            if (input.length > maxLength) {
                var truncated = $filter('limitTo')(input, maxLength);
                return truncated + continuation;
            }
            return input;
        };

    }]);
})();