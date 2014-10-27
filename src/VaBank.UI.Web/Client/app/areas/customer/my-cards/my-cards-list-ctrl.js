(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('myCardsListController', myCardsListController);

    myCardsListController.$inject = ['$scope', 'myCardsService'];

    function myCardsListController($scope, myCardsService) {

    }
})();