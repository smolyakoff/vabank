(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('exchangeRateService', exchangeRateService);

    exchangeRateService.$inject = ['$resource'];

    function exchangeRateService($resource) {

        var ExchangeRate = $resource('/api/exchange-rates', null, {
        });

        var Currency = $resource('/api/currencies', null, {            
            
        });

        return {            
            ExchangeRate: ExchangeRate,
            Currency: Currency
        };

    }
})();