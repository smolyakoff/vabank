(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('myCardsListController', myCardsListController);

    myCardsListController.$inject = ['$scope', 'myCardsService', 'data'];

    function myCardsListController($scope, myCardsService, data) {

        $scope.cards = data;
    }
})();