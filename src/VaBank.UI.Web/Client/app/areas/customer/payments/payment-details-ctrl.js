(function() {
    'use strict';

    angular.module('vabank.webapp').controller('paymentDetailsController', paymentDetailsController);

    paymentDetailsController.$inject = ['$scope', 'data'];
    function paymentDetailsController($scope, data) {
        $scope.details = data;
    }
})();