(function () {
    'use strict';

    angular
        .module('vabank.auth')
        .factory('authHttpInterceptor', authHttpInterceptor);

    authHttpInterceptor.$inject = ['$injector', '$q', '$location', 'authConfig', 'localStorageService'];

    function authHttpInterceptor($injector, $q, $location, authConfig, localStorageService) {

        var retryHttpRequest = function (config, deferred) {
            var $http = $injector.get('$http');
            $http(config).then(function(response) {
                deferred.resolve(response);
            }, function(response) {
                deferred.reject(response);
            });
        };
        
        var refreshToken = function () {
            function onSuccess(response) {
                localStorage.set(authConfig.storageKey, response);
                deferred.resolve(response);
            }

            function onError(response) {
                logout();
                deferred.reject(response);
            }
            var deferred = $q.defer();

            var token = localStorage.get(authConfig.storageKey);
            if (!_.isObject(token)) {
                deferred.reject();
            } else {
                var data = 'grant_type=refresh_token=' + token.refresh
                    + '&client_id=' + authConfig.clientId;
                $http({
                    url: authConfig.apiUrl,
                    method: 'POST',
                    data: data,
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                    }
                }).success(onSuccess).error(onError);
            }
            return deferred.promise;
        };


        var onRequest = function (config) {
            var token = localStorageService.get(authConfig.storageKey);
            var headers = config.headers || {};
            if (_.isObject(token)) {
                headers['Authorization'] = 'Bearer ' + token.access_token;
            }
            return config;            
        };

        var onResponseError = function (rejection) {
            function onRefreshSuceces() {
                retryHttpRequest(rejection.config, deferred);
            }

            function onRefreshFailure() {
                localStorageService.remove(authConfig.storageKey);
                $location.path(authConfig.loginUrl);
                deferred.reject(rejection);
            }

            var deferred = $q.defer();
            if (rejection.status !== 401) {
                deferred.reject(rejection);
            } else {
                refreshToken().then(onRefreshSuceces, onRefreshFailure);
            }
            return deferred.promise;
        };
        
        
        return {            
            request: onRequest,
            responseError: onResponseError
        };

    }
})();