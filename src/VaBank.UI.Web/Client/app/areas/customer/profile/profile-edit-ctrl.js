(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('profileEditController', profileEditController);

    profileEditController.$inject = ['$scope', 'profileService']; 

    function profileEditController($scope, profileService) {
        $scope.profileForm = {

        };
    }
})();
