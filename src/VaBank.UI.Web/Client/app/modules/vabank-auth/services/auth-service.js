(function () {
    'use strict';

    angular
        .module('vabank.auth')
        .factory('authService', authService);

    authService.$inject = ['$http', '$q', 'localStorageService', 'authConfig'];

    function authService($http, $q, localStorage, authConfig) {

        var User = (function () {

            var roles = [];
            var isAuthenticated = false;
            
            function UserImpl(token) {
                isAuthenticated = _.isObject(token);
                if (isAuthenticated) {
                    this.id = token.userId;
                    this.name = token.userName;
                    roles = JSON.parse(token.roles);
                }
            }
            UserImpl.prototype.isAuthenticated = function() {
                return isAuthenticated;
            };
            UserImpl.prototype.isInRole = function (role) {
                return _.contains(roles, role);
            };
            UserImpl.prototype.getRoles = function() {
                return angular.copy(roles);
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

            return deferred.promise;
        };

        var refreshToken = function() {
            function onSuccess(response) {
                localStorage.set(authConfig.storageKey, response);
                deferred.resolve(response);
            }

            function onError(response) {
                localStorage.remove(authConfig.storageKey);
                deferred.reject(response);
            }

            var deferred = $q.defer();
            var token = localStorage.get(authConfig.storageKey);

            if (!_.isObject(token)) {
                deferred.reject('Not Authenticated');
            } else {
                var data = 'grant_type=refresh_token'
                    + '&client_id=' + authConfig.clientId
                    + '&refresh_token=' + token.refresh_token;
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


        return {            
            login: login,
            logout: logout,
            getUser: getUser,
            refreshToken: refreshToken
        };

    }
})();