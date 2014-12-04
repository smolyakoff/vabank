(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('adminController', adminController);

    adminController.$inject = ['$scope', '$state', 'authService']; 

    function adminController($scope, $state, authService) {

        $scope.user = authService.getUser();

        $scope.logout = function() {
            authService.logout();
            $state.go('login');
        };

    }
})();
