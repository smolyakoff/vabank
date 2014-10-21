(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('cardsListController', cardsListController);

    cardsListController.$inject = ['$scope', 'cardsService'];

    function cardsListController($scope, cardsService) {
        $scope.cardsListForm = {
            

        };
        $scope.block = '';
        $scope.balance = '';
        $scope.name = '';
        $scope.cards = [
        { id: 1, block: 'Active', balance: "1260000", name: "Для любимой" },
        { id: 2, block: 'Active', balance: "432000", name: "Для себя" }
        ];
    }
})();