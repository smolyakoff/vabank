(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('routingResolve', routingInterceptor);

    routingInterceptor.$inject = ['$q'];

    function routingInterceptor($q) {

        var resolveOrDefault = function(promise, defaultValue) {
            var deferred = $q.defer();
            promise.then(function (response) {
                deferred.resolve(response);
            }, function (response) {
                deferred.resolve(defaultValue);
            });
            return deferred.promise;
        };

        var resolveAll = function (promises, names) {
            return $q.all(promises).then(function (arr) {
                var data = {};
                for (var i = 0; i < arr.length; i++) {
                    data[names[i]] = arr[i];
                }
                return data;
            });
        };

        return {            
            resolveAll: resolveAll,
            resolveOrDefault: resolveOrDefault
        };
    }
})();