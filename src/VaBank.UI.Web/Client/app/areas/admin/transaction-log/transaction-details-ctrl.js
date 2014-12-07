(function () {
    'use strict';

    var app = angular.module('vabank.webapp');
    app.controller('transactionDetailsController', exceptionDetails);

    exceptionDetails.$inject = ['$scope', 'data'];

    function exceptionDetails($scope, data) {
        $scope.entry = data.entry;
        $scope.account = data.entry.account;
        $scope.lastVersion = _.chain(data.entry.versions)
            .sortBy('timestampUtc')
            .last()
            .value();
        $scope.transactionCurrency = _.findWhere(data.currencies, { isoName: $scope.lastVersion.currencyISOName });

    }

})();
