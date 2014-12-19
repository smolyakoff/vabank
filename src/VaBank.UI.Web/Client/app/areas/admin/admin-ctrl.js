(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('adminController', adminController);

    adminController.$inject = ['$scope', '$state', 'authService']; 

    function adminController($scope, $state, authService) {

        $scope.user = authService.getUser();

        $scope.logout = function() {
            $state.go('login');
            authService.logout();
        };

        $scope.isDataManagementRoute = function () {
            return /management/i.test($state.current.name);
        };

        $scope.isLogRoute = function() {
            return /log/i.test($state.current.name);
        };
    }
})();
