(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('changePasswordController', changePasswordController);

    changePasswordController.$inject = ['$scope', 'profileService']; 

    function changePasswordController($scope, profileService) {
        $scope.changePwdForm = {

        };

    }
})();
