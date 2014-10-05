(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('userManagementController', userManagementController);

    userManagementController.$inject = ['$scope', 'userManagementService']; 

    function userManagementController($scope, userManager) {
    }
})();
