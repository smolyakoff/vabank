(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('validationService', validationService);

    validationService.$inject = ['$http', '$q'];

    function validationService($http, $q) {

        var API_URL = '/api/validate/';

        var camelCase = function(input) {
            return input[0].toLowerCase() + input.substring(1);
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
        
        var validators = {
            'userName': server,
            'phone': server,
            'password': server,
            'passwordConfirmation': passwordConfirmation
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
            _.forEach(faults, function (fault) {
                var exisingMessage = map[fault.propertyName] || '';
                map[camelCase(fault.propertyName)] = exisingMessage + fault.message;
            });
            return map;
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
                }
                return response;
            }
            var deferred = $q.defer();
            promise.then(onSuccess, onFailure);
            return deferred.promise;
        };

        return {
            getValidator: getValidator,
            getOptionalValidator: getOptionalValidator,
            getConditionalValidator: getConditionalValidator,
            toValidationMap: toValidationMap,
            handleServerResponse: handleServerResponse
        };
    }
})();