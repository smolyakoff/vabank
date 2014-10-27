(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('transferController', transferController);

    transferController.$inject = ['$scope', 'cardsService'];

    function transferController($scope, cardsService) {
    }
})();