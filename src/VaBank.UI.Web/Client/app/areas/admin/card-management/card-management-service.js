(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('cardManagementService', cardManagementService);

    cardManagementService.$inject = ['$resource', 'dataUtil', 'uiTools', 'userManagementService'];

    function cardManagementService($resource, dataUtil, uiTools, userManagementService) {

        return {
            User: userManagementService.User
        };
    }
})();