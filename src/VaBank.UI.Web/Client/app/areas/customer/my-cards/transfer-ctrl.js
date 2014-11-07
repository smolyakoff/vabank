(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('transferController', transferController);

    transferController.$inject = ['$scope', '$timeout', 'data'];

    function transferController($scope, $timeout, data) {

        $scope.cards = data.cards;

        $scope.isOneCard = data.cards.length == 1;

        $scope.smsConfirmationEnabled = data.profile.smsConfirmationEnabled;

        $scope.transferForm = {
            cardSource: data.cards.length == 1 ? 'vabank' : 'my',
            fromCardId: data.cards[0].cardId,
            toCard: {}
        };

        $scope.getSourceCard = function () {
            var fromCardId = $scope.transferForm.fromCardId;
            if (!fromCardId) {
                return {currency: {}};
            }
            return _.findWhere($scope.cards, { cardId: fromCardId });
        };

        $scope.$watch('transferForm.fromCardId', function (val) {
            if ($scope.transferForm.toCardId === val) {
                $timeout(function () {
                    $scope.transferForm.toCardId = null;
                });
            }
        });

    }
})();