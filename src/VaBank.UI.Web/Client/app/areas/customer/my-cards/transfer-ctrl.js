(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('transferController', transferController);

    transferController.$inject = ['$scope', '$timeout', 'uiTools', 'WizardHandler', 'myCardsService', 'data'];

    function transferController($scope, $timeout, uiTools, wizardHandler, myCardsService, data) {

        var validate = uiTools.validate;
        var SecurityCode = myCardsService.SecurityCode;

        var stepIndex = 0;
        var steps = ['fill', 'approve'];

        var currentStep = function() {
            return steps[stepIndex];
        };

        var isVabankTransfer = function (value, model) {
            return model.cardSource == 'vabank';
        };

        var defaultForm = {
            cardSource: data.cards.length == 1 ? 'vabank' : 'my',
            fromCardId: data.cards[0].cardId,
            toCard: {},
            toCardId: null,
            amount: 5,
            securityCode: {}
        };

        $scope.cards = data.cards;
        $scope.isOneCard = data.cards.length == 1;
        $scope.smsConfirmationEnabled = data.profile.smsConfirmationEnabled;
        $scope.smsCodeSent = false;
        $scope.transferFormController = {};
        $scope.sourceCard = {};
        $scope.destinationCard = {};

        $scope.canApprove = function () {
            if (!$scope.smsConfirmationEnabled) {
                return true;
            }
            var securityCode = $scope.transferForm.securityCode;
            if (_.isUndefined(securityCode)) {
                return false;
            }
            var smsCode = securityCode.code;
            return _.isString(smsCode) && smsCode.length === 6;
        };

        $scope.transferForm = angular.copy(defaultForm); 

        $scope.validationRules = {
            toCardId: { custom: validate.getConditionalValidator('required', _.negate(isVabankTransfer)) },
            toCard: {
                expiration: { custom: validate.getConditionalValidator('cardExpiration', isVabankTransfer) },
                cardNumber: { custom: validate.getConditionalValidator('cardNumber', isVabankTransfer) }
            },
            securityCode: {
                code: {
                    custom: validate.getConditionalValidator('required', function () {
                        return $scope.smsConfirmationEnabled && currentStep() == 'approve';
                    })
                }
            },
            amount: {
                custom: validate.combineValidators([
                    validate.getValidator('min', {
                        value: 5,
                        message: 'Минимальная сумма перевода: {0}.'
                    }),
                    validate.getValidator('max', {
                        value: function() {
                            return $scope.getSourceCard().balance;
                        },
                        message: 'Недостаточно средств на счете.'
                    })
                ])
            }
        };

        $scope.sendSmsCode = function () {
            function onSuccess(code) {
                $scope.smsCodeSent = true;
                $scope.transferForm.securityCode.id = code.id;
                uiTools.notify({
                   type: 'info',
                   message: 'Введите код подтверждения в поле смс-код' 
                });
            };
            SecurityCode.generate().then(onSuccess);
        };

        $scope.getSourceCard = function () {
            var fromCardId = $scope.transferForm.fromCardId;
            if (!fromCardId) {
                return {currency: {}};
            }
            return _.findWhere($scope.cards, { cardId: fromCardId });
        };

        $scope.back = function() {
            wizardHandler.wizard('transferWizard').previous();
            stepIndex -= 1;
        };

        $scope.continue = function () {
            validate.handleManualValidation($scope.transferFormController).then(function () {
                stepIndex += 1;
                $scope.sourceCard = $scope.getSourceCard();
                $scope.destinationCard = $scope.transferForm.cardSource === 'my'
                    ? _.findWhere($scope.cards, { cardId: $scope.transferForm.toCardId })
                    : $scope.transferForm.toCard;
                wizardHandler.wizard('transferWizard').next();
            });
        };

        $scope.cancel = function() {
            wizardHandler.wizard('transferWizard').goTo(0);
            stepIndex = 0;
            _.extend($scope.transferForm, defaultForm);
            $scope.smsCodeSent = false;
            $scope.transferFormController.resetErrors();
        };

        $scope.approve = function () {
            stepIndex = 2;
            wizardHandler.wizard('transferWizard').goTo(stepIndex);
        };

        $scope.$watch('transferForm.fromCardId', function (val) {
            if ($scope.transferForm.toCardId === val) {
                $timeout(function () {
                    $scope.transferForm.toCardId = null;
                    $scope.transferFormController.resetField('toCardId');
                });
            }
        });

    }
})();