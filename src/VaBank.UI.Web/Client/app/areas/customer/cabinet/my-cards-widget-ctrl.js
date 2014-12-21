(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('myCardsWidgetController', myCardsWigetController);

    myCardsWigetController.$inject = ['$scope', 'uiTools', 'screenSize', 'myCardsService'];

    function myCardsWigetController($scope, uiTools, screenSize, myCardsService) {

        var Card = myCardsService.Card;
        
        var pageSize = function() {
            return screenSize.is('lg') ? 2 : 1;
        };

        var createSlides = function(cards) {
            var size = pageSize();
            var pages = _.groupBy(cards, function (x, i) {
                return Math.floor(i / size);
            });
            var slides = _.map(pages, function (v) {
                return { cards: v };
            });
            return slides;
        };

        var cards = [];

        $scope.screen = null;

        $scope.loading = uiTools.promiseTracker();

        $scope.slides = [];

        $scope.isExpired = function(card) {
            
        };
        
        var init = function () {
            var promise = Card.query().$promise.then(function (data) {
                cards = data;
                $scope.slides = createSlides(data);
            });
            $scope.loading.addPromise(promise);
            screenSize.on('xs, sm, md, lg', function(size) {
                $scope.slides = createSlides(cards);
                $scope.screen = size;
            });
        };
        init();
    }
})();