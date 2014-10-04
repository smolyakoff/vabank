(function() {
    'use strict';
    
    angular.module('vabank.webapp').factory('httpErrorNotifierInterceptor', httpErrorNotifierInterceptor);

    httpErrorNotifierInterceptor.$inject = ['$q', '$rootScope', 'serverInfo', 'errorResources'];

    function httpErrorNotifierInterceptor($q, $rootScope, serverInfo, errorResources) {
        

        var getMessage = function(response) {
            if (serverInfo.isDebug) {
                var data = response.data || {};

                return {
                    type: 'error',
                    message: '<b>' + data.exceptionType + '</b><br>' + data.exceptionMessage,
                    title: 'HTTP ' + response.status
                };
            } else {
                return {
                    type: 'error',
                    message: errorResources["HTTP500.Message"],
                    title: errorResources["HTTP500.Title"],
                };
            }
        };

        var requestError = function (rejection) {
            if (rejection.status.toString()[0] === '5' && !$rootScope.stateChanging) {
                $rootScope.$broadcast('notificationPushed', getMessage(rejection));
            }
            return $q.reject(rejection);
        };

        return {
            responseError: requestError
        };
    }
})();