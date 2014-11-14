(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('accountStatementController', accountStatementController);

    accountStatementController.$inject = ['$scope', 'myCardsService', 'data'];

    function accountStatementController($scope, myCardsService, data) {
        $scope.card = {};
        $scope.date = {};

        $scope.cards = data.cards;
        $scope.dates = [
            { name: 'Сегодня'},
            { name: 'Вчера'},
            { name: 'За последние 7 дней' },
            { name: 'За последний месяц'}
        ];


    $scope.clear = function () {
            $scope.card.selected = undefined;
            $scope.date.selected = undefined;
        };
    }
})();