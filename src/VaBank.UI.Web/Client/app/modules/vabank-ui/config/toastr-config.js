(function () {
    'use strict';

    var module = angular.module('vabank.ui');

    module.config(['localStorageServiceProvider', function (localStorageServiceProvider) {
        localStorageServiceProvider.setPrefix('vabank.ui');
    }]);
})();