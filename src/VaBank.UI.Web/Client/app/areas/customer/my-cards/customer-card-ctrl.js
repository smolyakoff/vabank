(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('customerCardController', customerCardController);

    customerCardController.$inject = ['$scope', 'uiTools', 'myCardsService'];

    function customerCardController($scope, uiTools, myCardsService) {

        var Card = myCardsService.Card;

        var card = $scope.card;
        var pristineCard = angular.copy(card);

        $scope.limitsForm = {};

        $scope.limits = card.cardLimits;

        $scope.limitsHidden = true;
        $scope.nameEdit = false;

        $scope.setCardBlock = function(block) {
            Card.block({ cardId: card.cardId }, { blocked: block })
                .$promise.then(function(response) {
                    uiTools.notify({
                        type: block ? 'warning': 'success',
                        message: response.message
                    });
                card.blocked = block;
            });
        };

        $scope.toggleEditName = function(edit) {
            $scope.nameEdit = edit;
        };

        $scope.cancelEditName = function() {
            card.friendlyName = pristineCard.friendlyName;
            $scope.toggleEditName(false);
        };

        $scope.saveName = function () {
            var settings = {friendlyName: card.friendlyName};
            Card.updateSettings({cardId: card.cardId}, settings).$promise.then(function() {
                uiTools.notify({
                    type: 'success',
                    message: 'Имя карты было успешно обновлено'
                });
                $scope.toggleEditName(false);
            });
        };

        $scope.cancelLimits = function() {
            angular.extend($scope.limits, pristineCard.cardLimits);
        };

        $scope.updateLimits = function () {
            Card.updateSettings({ cardId: card.cardId }, {
                cardLimits: $scope.limits
            }).$promise.then(function(response) {
                uiTools.notify({                    
                    type: 'success',
                    message: 'Лимиты были успешно обновлены'
                });
            });
        };
    }
})();