(function () {
    'use strict';

    angular
        .module('vabank.auth')
        .factory('authService', authService);

    authService.$inject = ['$http', '$q', 'localStorageService', 'authConfig'];

    function authService($http, $q, localStorage, authConfig) {

        var User = (function () {
            
            function UserImpl(token) {
                this.token = token;
            }

            UserImpl.isAuthenticated = function() {
                return true;
            };

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



        return {            
            login: login,
            logout: logout,
            getUser: getUser
        };

    }
})();