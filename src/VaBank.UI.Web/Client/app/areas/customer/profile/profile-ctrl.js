(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('profileController', profileController);

    profileController.$inject = ['$scope', 'profileService']; 

    function profileController($scope, profileService) {
    }
})();
