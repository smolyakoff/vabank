(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('validationService', validationService);

    validationService.$inject = ['$http', '$q', '$filter', '$interpolate', 'NestedObjectHelper'];

    function validationService($http, $q, $filter, $interpolate, nested) {

        var API_URL = '/api/validate/';

        var format = $filter('stringFormat');

        var camelCase = function (input) {
            var props = input.split('.');
            var camelCased = _.map(props, function(x) {
                return x[0].toLowerCase() + x.substring(1);
            });
            return camelCased.join('.');
        };

        var constructMessage = function(faults) {
            var messages = _.pluck(faults, 'message');
            return messages.join(' ');
        };

        var server = function (name) {
            return function (value) {
                var deferred = $q.defer();
                $http({
                    method: 'POST',
                    url: API_URL + name,
                    data: JSON.stringify(value)
            }).success(onSuccess).error(onError);

                function onSuccess(data) {
                    if (data.isValid) {
                        deferred.resolve();
                    } else {
                        var message = constructMessage(data.faults);
                        deferred.reject(message);
                    }
                }

                function onError(data, status) {
                    deferred.reject('Server validation failed.');
                    if (status == 404) {
                        throw new Error('Server validator [' + name + '] was not found.');
                    }
                }

                return deferred.promise;
            };
        };

        var passwordConfirmation = function (name, options) {
            var passwordField = options.passwordField;
            return function (value, model) {
                var deferred = $q.defer();
                
                if (value === model[passwordField]) {
                    deferred.resolve();
                } else {
                    deferred.reject('Пароли должны совпадать');
                };
                return deferred.promise;
            };
        };
        
        var asConditional = function (validator, predicate) {
            return function (value, model) {
                var deferred = $q.defer();
                if (!predicate(value, model)) {
                    deferred.resolve();
                    return deferred.promise;
                }
                return validator(value, model);
            };
        };

        var asOptional = function (validator) {
            var condition = function(value, model) {
                return !((_.isString(value) && value.trim() === '') ||
                    _.isUndefined(value) || 
                    _.isNull(value));
            };
            return asConditional(validator, condition);
        };
        
        var createValidator = function (checker) {
            return function (value, model, options) {
                var deferred = $q.defer();
                var message = checker(value, model, options);
                if (_.isString(message)) {
                    deferred.reject(message);
                }
                deferred.resolve();
                return deferred.promise;
            };
        };

        var required = function(name, options) {
            options = _.defaults(options || {}, {                
                message: 'Обязательное поле'
            });
            return createValidator(function(value) {
                if (_.isNull(value) || _.isUndefined(value)) {
                    return options.message;
                }
                if (value.toString().length === 0) {
                    return options.message;
                }
                return null;
            });
        };

        var min = function(name, options) {
            return createValidator(function (value, model) {
                options = _.defaults(options, {
                    message: 'Значение не должно быть меньше {0}.'
                });
                var minValue = _.isFunction(options.value)
                    ? options.value(value, model)
                    : options.value;
                if (value < minValue) {
                    return format(options.message, minValue);
                }
                return null;
            });
        };
        
        var max = function (name, options) {
            return createValidator(function (value, model) {
                options = _.defaults(options, {
                    message: 'Значение не должно превышать {0}.'
                });
                var maxValue = _.isFunction(options.value)
                    ? options.value(value, model)
                    : options.value;
                if (value > maxValue) {
                    return format(options.message, maxValue);
                }
                return null;
            });
        };

        var cardNumber = function() {
            return createValidator(function (value) {
                var typeCodes = ['3', '4', '5', '6'];
                var vabankCode = '666';
                var isCreditCard = _.isString(value) &&
                    value.length === 16 &&
                    _.contains(typeCodes, value[0]);
                if (!isCreditCard) {
                    return 'Неверный номер карты.';
                }
                if (value.substr(1, 3) != vabankCode) {
                    return 'Неверный номер карты VaBank.';
                }
                return null;
            });
        };

        var cardExpiration = function() {
            return createValidator(function (value) {
                if (!_.isString(value)) {
                    return 'Не указан срок действия.';
                }
                if (value.length != 4) {
                    return 'Неверный срок действия.';
                }
                var now = moment();
                var month = value.substr(0, 2);
                var monthValue = parseInt(month);
                if (monthValue <= 0 || month > 12) {
                    return 'Неверно указан месяц.';
                }
                var year = value.substr(2, 2);
                var yearValue = 2000 + parseInt(year);
                if (yearValue < 0) {
                    return 'Неверно указан год';
                }
                var expiration = moment({ year: yearValue, month: monthValue });
                if (expiration.isBefore(now)) {
                    return 'Неверный срок действия';
                }
                return null;
            });
        };
        
        var validators = {
            'required': required,
            'userName': server,
            'phone': server,
            'password': server,
            'passwordConfirmation': passwordConfirmation,
            'min': min,
            'max': max,
            'cardNumber': cardNumber,
            'cardExpiration': cardExpiration
        };

        var getValidator = function(name, options) {
            if (!_.isFunction(validators[name])) {
                throw new Error('Validator [' + name + '] was not found');
            }
            var factory = validators[name];
            return factory(name, options);
        };

        var getOptionalValidator = function(name, options) {
            var validator = getValidator(name, options);
            return asOptional(validator);
        };

        var getConditionalValidator = function(name, predicate, options) {
            var validator = getValidator(name, options);
            return asConditional(validator, predicate);
        };

        var toValidationMap = function (faults) {
            if (!_.isArray(faults)) {
                return {};
            }
            var map = {};
            _.each(faults, function (fault) {
                var camelCased = camelCase(fault.propertyName);
                var exisingMessage = map[camelCased] || '';
                _.deep(map, camelCased, exisingMessage + fault.message);
            });
            return map;
        };

        var handleManualValidation = function (formForController) {
            var deferred = $q.defer();
            var onFailure = function (errorMap) {
                _.each(errorMap, function(map) {
                    var properties = nested.flattenObjectKeys(map);
                    _.each(properties, function (x) {
                        formForController.setFieldError(x, nested.readAttribute(map, x));
                    });
                });
                deferred.reject(errorMap);
                return errorMap;
            };

            var onSuccess = function() {
                deferred.resolve();
            };
            formForController.validateForm().then(onSuccess, onFailure);
            return deferred.promise;
        };

        var handleServerResponse = function (promise) {
            function onSuccess(data) {
                deferred.resolve(data);
                return data;
            }
            
            function onFailure(response) {
                if (response.status === 400 && response.data.errorType && response.data.errorType === 'validation') {
                    var map = toValidationMap(response.data.faults);
                    deferred.reject(map);
                    return map;
                } else {
                    deferred.reject(response);
                }
                return response;
            }
            var deferred = $q.defer();
            promise.then(onSuccess, onFailure);
            return deferred.promise;
        };

        var setFormErrors = function(formForController, errorMap) {
            _.each(errorMap, function (map) {
                var properties = nested.flattenObjectKeys(map);
                _.each(properties, function (x) {
                    formForController.setFieldError(x, nested.readAttribute(map, x));
                });
            });
        };

        var combineValidators = function (validatorArray) {
            if (!_.isArray(validatorArray)) {
                throw new TypeError("Array expected!");
            }
            return function(value, model) {
                var deferred = $q.defer();
                var promises = _.map(validatorArray, function(x) {
                    return x(value, model);
                });
                $q.all(promises).then(function() {
                    deferred.resolve();
                }, function(m) {
                    deferred.reject(m);
                });
                return deferred.promise;
            };
        };

        return {
            getValidator: getValidator,
            getOptionalValidator: getOptionalValidator,
            getConditionalValidator: getConditionalValidator,
            createValidator: createValidator,
            combineValidators: combineValidators,
            toValidationMap: toValidationMap,
            handleServerResponse: handleServerResponse,
            handleManualValidation: handleManualValidation,
            setFormErrors: setFormErrors
        };
    }
})();