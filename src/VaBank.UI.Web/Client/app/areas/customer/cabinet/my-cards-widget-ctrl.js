(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('myCardsWidgetController', myCardsWigetController);

    myCardsWigetController.$inject = ['$scope', 'uiTools', 'responsiveHelper', 'myCardsService'];

    function myCardsWigetController($scope, uiTools, responsive, myCardsService) {

        var Card = myCardsService.Card;

        $scope.loading = uiTools.promiseTracker();

        $scope.cards = [];

        $scope.isScreenLarge = function() {
            return responsive.isLg();
        }

        $scope.slides = function() {
        }

        var init = function () {
            var promise = Card.query().$promise.then(function (cards) {
                $scope.cards = cards;
            });
            $scope.loading.addPromise(promise);
        };

        init();
    }
})();