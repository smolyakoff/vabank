(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('validationService', validationService);

    validationService.$inject = ['$http', '$q'];

    function validationService($http, $q) {

        var API_URL = '/api/validate/';

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

        var validators = {
            'login': server('login'),
            'password': function(value) {
                return $q.reject('TODO');
            }
        };

        var getValidator = function(name) {
            if (!_.isFunction(validators[name])) {
                throw new Error('Validator [' + name + '] was not found');
            }
            return validators[name];
        };

        return {
            getValidator: getValidator
        };
    }
})();