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

        $scope.disabled = card.isExpired();

        $scope.settingsForm = {            
          cardLimits: card.cardLimits  
        };

        $scope.limitValidationRules = {
            cardLimits: {
                amountPerDayLocal: { custom: uiTools.validate.getValidator('min', {value: 0}) },
                amountPerDayAbroad: { custom: uiTools.validate.getValidator('min', {value: 0}) },
                operationsPerDayLocal: { custom: uiTools.validate.getValidator('min', {value: 0}) },
                operationsPerDayAbroad: { custom: uiTools.validate.getValidator('min', {value: 0}) }
            }
        };

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

        $scope.cancelLimits = function () {
            $scope.settingsForm.cardLimits = angular.copy(pristineCard.cardLimits);
        };

        $scope.updateLimits = function () {
            return uiTools.dialog.confirmation({
                message: 'Вы действительно хотите обновить лимиты по карте?',
                title: 'Необходимо подтверждение действия'
            }).result.then(function (isConfirmed) {
                if (isConfirmed) {
                    var promise = Card.updateSettings(
                        { cardId: card.cardId },
                        $scope.settingsForm).$promise;
                    return uiTools.validate.handleServerResponse(promise);
                }
                return null;
            });
        };

        $scope.localCountLimitBlur = function () {
            if (!_.isNumber($scope.settingsForm.cardLimits.operationsPerDayLocal)) {
                $scope.settingsForm.cardLimits.operationsPerDayLocal = 0;
            }
        };

        $scope.abroadCountLimitBlur = function() {
            if (!_.isNumber($scope.settingsForm.cardLimits.operationsPerDayAbroad)) {
                $scope.settingsForm.cardLimits.operationsPerDayAbroad = 0;
            }
        }

        $scope.onLimitsUpdated = function (data) {
            if (!data) {
                return;
            }
            uiTools.notify({
                type: 'success',
                message: 'Лимиты были успешно обновлены'
            });
            pristineCard.cardLimits = $scope.settingsForm.cardLimits;
        };
    }
})();