(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('paymentService', paymentService);

    paymentService.$inject = ['$q', '$resource', '$http', 'dataUtil', 'authService', 'validationService', 'myCardsService'];

    function paymentService($q, $resource, $http, dataUtil, authService, validationService, myCardsService) {

        var Payment = (function () {

            var getUserId = function () {
                return authService.getUser().id;
            }

            var defaults = {
                filter: function() {
                    return {
                        from: {
                            propertyName: 'dateUtc',
                            operator: dataUtil.filters.operator.GreaterThanOrEqual,
                            value: moment().startOf('month'),
                            propertyType: 'datetime'
                        },
                        to: {
                            propertyName: 'dateUtc',
                            operator: dataUtil.filters.operator.LessThan,
                            value: moment().startOf('day').add(1, 'day'),
                            propertyType: 'datetime'
                        }
                    }
                }
            }

            var PaymentImpl = $resource('/api/payments/:operationId', { operationId: '@operationId' }, {
                query: {
                    url: '/api/users/:userId/payments',
                    isArray: true,
                    params: {
                        userId: getUserId,
                        filter: dataUtil.filters.combine(defaults.filter(), dataUtil.filters.logic.And).toLINQ()
                    }
                },
                create: {
                    method: 'POST',
                    url: '/api/payments'
                },
                getTemplate: {
                    url: '/api/payment-templates/:code',
                    method: 'GET'
                },
                getPrototype: {
                    url: '/api/payments/:operationId/form',
                    method: 'GET'
                },
                getMostlyUsed: {
                    url: '/api/users/:userId/payments/mostly-used',
                    method: 'GET',
                    isArray: true,
                    params: {
                        userId: getUserId,
                        maxResults: 3,
                        from: function() {
                            return moment().utc().subtract(3, 'month').startOf('month').toJSON();
                        },
                        to: function() {
                            return moment().utc().toJSON();
                        }
                    }
                }
            });
            PaymentImpl.defaults = defaults;

            return PaymentImpl;

        })();

            
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