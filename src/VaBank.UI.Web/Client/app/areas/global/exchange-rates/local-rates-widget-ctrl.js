(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('localRatesWidgetController', localRatesWidgetController);

    localRatesWidgetController.$inject = ['$scope', 'uiTools', 'exchangeRateService'];

    function localRatesWidgetController($scope, uiTools, exchangeRateService) {

        var ExchangeRate = exchangeRateService.ExchangeRate;

        var priority = ['USD', 'EUR'];

        var getForeignPriority = function(rate) {
            var index = _.indexOf(priority, rate.foreignCurrency.isoName);
            return index < 0 ? 100 : index;
        };
        
        $scope.loading = uiTools.promiseTracker();

        $scope.rates = [];

        $scope.lastUpdatedUtc = null;

        var init = function() {
            var promise = ExchangeRate.query().$promise.then(function(rates) {
                $scope.rates = _.sortBy(rates, getForeignPriority);
                $scope.lastUpdatedUtc = _.chain(rates)
                    .pluck('timestampUtc')
                    .map(function (x) { return moment.utc(x).local(); })
                    .min(function (x) { return x.valueOf(); })
                    .value();
            });
            $scope.loading.addPromise(promise);
        };

        init();
    }
})();