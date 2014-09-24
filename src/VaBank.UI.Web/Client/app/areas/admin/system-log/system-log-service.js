(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('systemLogService', systemLogService);

    systemLogService.$inject = ['$resource'];

    function systemLogService($resource) {

        var logEntry = $resource('/api/logs/system', {}, {
            query: {
              isArray: true  
            },
            lookup: {
              url: '/api/logs/system/lookup',
              isArray: false
            }
        });

        return {            
          LogEntry: logEntry  
        };

    }
})();