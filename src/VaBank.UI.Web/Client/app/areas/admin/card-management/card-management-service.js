(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('cardManagementService', cardManagementService);

    cardManagementService.$inject = ['$resource', 'dataUtil', 'uiTools'];

    function cardManagementService($resource, dataUtil, uiTools) {

        return {            
        };
    }
})();