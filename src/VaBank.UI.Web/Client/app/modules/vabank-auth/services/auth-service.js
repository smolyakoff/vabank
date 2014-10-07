(function () {
    'use strict';

    angular
        .module('app')
        .factory('authService', authService);

    authService.$inject = ['$http', '$q', 'localStorageService', 'authConfig'];

    function authService($http, localStorage, authConfig) {

        var User = (function () {
            
            function UserImpl(token) {
                
            }

            return UserImpl;
        })();
        
        var getUser = function () {
            var token = localStorage.get(authConfig.storageKey);
            return new User(token);
        };
        
        var logout = function () {
            localStorage.remove(authConfig.storageKey);
        };
    
        var login = function (loginForm) {
            var deferred = $q.defer();

            var data = 'grant_type=password&username=' + loginForm.login
                     + '&password=' + loginForm.password
                     + '&client_id=' + authConfig.clientId;
            $http({
                url: authConfig.apiUrl,
                method: 'POST',
                data: data,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                }
            }).success(onSuccess).error(onError);
            
            function onSuccess(response) {
                localStorage.set(authConfig.storageKey, response);
                deferred.resolve(response);
            }
            
            function onError(response) {
                logout();
                deferred.reject(response);
            }
        };

        var refreshToken = function() {

        };

        return {            
            login: login,
            refreshToken: refreshToken,
            logout: logout,
            getUser: getUser
        };

    }
})();