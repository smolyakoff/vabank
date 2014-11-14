(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('securityCodeService', securityCodeService);

    securityCodeService.$inject = ['$q', '$http'];

    function securityCodeService($q, $http) {

        var generate = function () {
            function onSuccess(response) {
                deferred.resolve(response);
                return response.data;
            }
            function onFailure(response) {
                deferred.reject(response);
                return response;
            }
            var deferred = $q.defer();
            $http({ method: 'POST', url: '/api/security-codes' })
                .success(onSuccess)
                .error(onFailure);
            return deferred.promise;
        };

        return {            
          generate: generate  
        };

    }
})();