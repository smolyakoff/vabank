(function() {
    'use strict';

    angular.module('vabank.webapp').controller('customerController', customerController);

    customerController.$inject = ['$window', '$rootScope', '$state', '$scope', 'authService'];

    function customerController($window, $rootScope, $state, $scope, authService) {

        $scope.back = function() {
            $window.history.back();
        };
        
        $scope.forward = function () {
            $window.history.forward();
        };

        $scope.logout = function () {
            authService.logout();
            $state.go('login');
        };

        $scope.$on('$stateChangeSuccess', function(event, toState) {
            if ($state.includes('customer')) {
                $scope.header = toState.data.subtitle;
            }
        });
    }
    
})();