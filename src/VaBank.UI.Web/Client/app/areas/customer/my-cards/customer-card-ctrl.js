(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('customerCardController', customerCardController);

    customerCardController.$inject = ['$scope'];

    function customerCardController($scope) {

        var card = $scope.card;
        $scope.limits = card.cardLimits;

        $scope.limitsHidden = true;
        $scope.nameEdit = false;

        $scope.editName = function() {
            $scope.nameEdit = true;
        };
    }
})();