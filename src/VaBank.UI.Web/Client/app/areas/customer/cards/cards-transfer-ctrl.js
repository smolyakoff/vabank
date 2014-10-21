(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('cardsTransferController', cardsTransferController);

    cardsTransferController.$inject = ['$scope', 'cardsService'];

    function cardsTransferController($scope, cardsService) {
        $scope.cardsTransferForm = {

        };

    }
})();