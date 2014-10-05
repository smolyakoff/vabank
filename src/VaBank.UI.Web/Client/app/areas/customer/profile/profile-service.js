(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('profileService', profileService);

    profileService.$inject = ['$resource'];

    function profileService($resource) {
        return {            
            
        };
    }
})();