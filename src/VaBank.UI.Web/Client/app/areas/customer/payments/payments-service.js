(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('paymentService', paymentService);

    paymentService.$inject = ['$q', '$resource', '$http', 'validationService', 'myCardsService'];

    function paymentService($q, $resource, $http, validationService, myCardsService) {

        var Payment = $resource('/api/payments/:code', { code: '@code' }, {
           create: {
               method: 'POST',
               url: '/api/payments'
           },
           getTemplate: {
               url: '/api/payments/:code/template',
               method: 'GET'
           } 
        });

        var PaymentForm = (function () {
            var parseValidator = function(options, key) {
                if (key === 'pattern') {
                    options.rule = new RegExp(options.rule);
                }
                return [key, options];
            }

            var create = function (formTemplate) {
                var form = {};
                var validators = {};
                _.each(formTemplate, function (template, key) {
                    form[key] = template.default;
                    var validation = template.editor.options.validation;
                    if (_.isObject(validation)) {
                        validators[key] = _.object(_.map(validation, parseValidator));
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