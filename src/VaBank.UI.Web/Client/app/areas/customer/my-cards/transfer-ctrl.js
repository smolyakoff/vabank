(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('transferController', transferController);

    transferController.$inject = ['$scope', '$timeout', '$state', 'uiTools', 'WizardHandler', 'myCardsService', 'data'];

    function transferController($scope, $timeout, $state, uiTools, wizardHandler, myCardsService, data) {
        
        if (data.cards.length === 0) {
            uiTools.notify({
                type: 'warning',
                message: 'Нет карт с которых разрешен перевод.'
            });
            $state.go('customer.cards.list');
        }

        var validate = uiTools.validate;
        var SecurityCode = myCardsService.SecurityCode;
        var Transfer = myCardsService.Transfer;

        var stepIndex = 0;
        var steps = ['fill', 'approve'];

        var currentStep = function() {
            return steps[stepIndex];
        };

        var isVabankTransfer = function (value, model) {
            return model.cardSource == 'vabank';
        };

        var minimalAmounts = data.lookup.minimalAmountsByCurrency;

        $scope.cards = data.cards;
        $scope.cannotDoPersonalTransfer = data.cards.length == 1 || _.all(data.cards, function(x) {
            return x.accountNo === $scope.cards[0].accountNo;
        });
        
        var defaultForm = {
            cardSource: $scope.cannotDoPersonalTransfer ? 'vabank' : 'my',
            fromCardId: data.cards[0].cardId,
            toCard: {},
            toCardId: null,
            amount: 5,
            securityCode: {}
        };

        $scope.errorMessages = [];
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
                        value: function() {
                            return minimalAmounts[$scope.getSourceCard().currency.isoName.toLowerCase()];
                        },
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
                $scope.transferForm.securityCode.id = code.id;
                uiTools.notify({
                   type: 'info',
                   message: 'Введите код подтверждения в поле смс-код' 
                });
                $scope.smsCodeSent = true;
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

        $scope.canBeDestinationCard = function(card) {
            var source = $scope.getSourceCard();
            if (!_.isString(source.accountNo)) {
                return true;
            }
            return source.accountNo != card.accountNo;
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
            function onSuccess() {
                stepIndex = 2;
                wizardHandler.wizard('transferWizard').goTo(stepIndex);
            }
            function onError(response) {
                var defaultMessage = 'Невозможно выполнить операцию';
                if (response.status === 400 && response.data.errorType === 'validation') {
                    $scope.errorMessages = _.pluck(response.data.faults, 'message');
                } else if (response.status !== 500) {
                    $scope.errorMessages = [response.data.message || defaultMessage];
                } else {
                    $scope.errorMessages = [defaultMessage];
                }
                stepIndex = 3;
                wizardHandler.wizard('transferWizard').goTo(stepIndex);
            }

            var transfer = {
                type: $scope.transferForm.cardSource == 'my' ? 'personal' : 'interbank',
                fromCardId: $scope.transferForm.fromCardId,
                amount: $scope.transferForm.amount
            };
            if ($scope.smsConfirmationEnabled) {
                transfer.securityCode = $scope.transferForm.securityCode;
            }
            if (transfer.type === 'personal') {
                transfer.toCardId = $scope.transferForm.toCardId;
            } else {
                transfer.toCardNo = $scope.transferForm.toCard.cardNumber;
                var month = parseInt($scope.transferForm.toCard.expiration.substr(0, 2));
                var year = 2000 + parseInt($scope.transferForm.toCard.expiration.substr(2, 2));
                transfer.toCardExpirationDateUtc = moment.utc([year, month]).toJSON();
            }
            var promise = Transfer.create(transfer).$promise;
            promise.then(onSuccess, onError);
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