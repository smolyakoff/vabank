(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('paymentService', paymentService);

    paymentService.$inject = ['$q', '$resource', '$http', 'validationService', 'myCardsService'];

    function paymentService($q, $resource, $http, validationService, myCardsService) {

        var PaymentForm = (function () {
            var validate = function (templateCode, form) {
                return $http({
                    method: 'POST',
                    url: '/api/validate/' + templateCode,
                    data: form
                }).then(function(response) {
                    return response.data;
                });
            };
            return {                
              validate: validate  
            };
        })();

        return {
            Card: myCardsService.Card,
            SecurityCode: myCardsService.SecurityCode,
            PaymentForm: PaymentForm
        };

    }
})();