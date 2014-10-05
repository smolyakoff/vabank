(function() {
    'use strict';

    angular.module('vabank.webapp').controller('cabinetController', cabinetController);

    cabinetController.$inject = ['$window', '$rootScope', '$state', '$scope'];

    function cabinetController($window, $rootScope, $state, $scope) {

        $scope.back = function() {
            $window.history.back();
        };
        
        $scope.forward = function () {
            $window.history.forward();
        };

        $scope.$on('$stateChangeSuccess', function(event, toState) {
            if ($state.includes('cabinet')) {
                $scope.header = toState.data.subtitle;
            }
        });
    }
    
})();