(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('paymentService', paymentService);

    paymentService.$inject = ['$q', '$resource', '$http', 'validationService', 'myCardsService'];

    function paymentService($q, $resource, $http, validationService, myCardsService) {

        var Payment = $resource('/api/payments/:code', { code: '@code' }, {            
           getTemplate: {
               url: '/api/payments/:code/template',
               method: 'GET'
           } 
        });

        var PaymentForm = (function () {
            var create = function (formTemplate) {
                var form = {};
                var validators = {};
                _.each(formTemplate, function(template, key) {
                    form[key] = template.value;
                    if (_.isObject(template.editor.validation)) {
                        validators[key] = template.editor.validation;
                    }
                });
                return {
                    form: form,
                    validators: validators
                };
            };
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
                validate: validate,
                create: create
            };
        })();

        return {
            Card: myCardsService.Card,
            SecurityCode: myCardsService.SecurityCode,
            PaymentForm: PaymentForm,
            Payment: Payment
        };

    }
})();