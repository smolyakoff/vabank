(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('cardsStatementController', cardsStatementController);

    cardsStatementController.$inject = ['$scope', 'cardsService'];

    function cardsStatementController($scope) {
        $scope.cardsStatementForm = {

    };

    }
})();