(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('adminController', adminController);

    adminController.$inject = ['$scope','authService']; 

    function adminController($scope, authService) {

        $scope.user = authService.getUser();

    }
})();
