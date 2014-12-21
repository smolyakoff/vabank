(function() {
    'use strict';

    angular.module('vabank.webapp').factory('systemStatsService', systemStatsService);

    systemStatsService.$inject = ['$http'];
    function systemStatsService($http) {

        var SystemStats = {
            query: function(params) {
                return $http({
                    url: '/api/system/stats/' + params.type,
                    method: 'GET'
                }).then(function(response) {
                    return response.data;
                });
            }
        }

        return {
            SystemStats: SystemStats
        }

    }
})();