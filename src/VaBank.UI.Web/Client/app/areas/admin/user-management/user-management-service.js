(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('userManagementService', userManagementService);

    userManagementService.$inject = ['$resource'];

    function userManagementService($resource) {

        return {            
            
        };
    }
})();