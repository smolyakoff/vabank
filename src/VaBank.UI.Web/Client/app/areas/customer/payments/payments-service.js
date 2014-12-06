(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('paymentService', paymentService);

    paymentService.$inject = ['$resource', 'myCardsService'];

    function paymentService($resource, myCardsService) {

        return {
            Card: myCardsService.Card,
            SecurityCode: myCardsService.SecurityCode,
        };

    }
})();