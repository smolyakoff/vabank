(function() {
    'use strict';

    angular.module('vabank.webapp').controller('paymentsArchiveController', paymentsArchiveController);

    function paymentsArchiveController($scope, $state, data) {

        $scope.payments = data;
        $scope.displayedPayments = angular.copy(data);

    }
    paymentsArchiveController.$inject = ['$scope', '$state', 'data'];


})();