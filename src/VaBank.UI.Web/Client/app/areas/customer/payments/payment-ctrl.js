(function () {
    'use strict';

    angular.module('vabank.webapp').controller('paymentController', paymentController);

    paymentController.$inject = ['$scope', '$q', '$timeout', 'uiTools', 'WizardHandler', 'paymentService', 'data'];

    function paymentController($scope, $q, $timeout, uiTools, wizardHandler, paymentService, data) {

        var PaymentForm = paymentService.PaymentForm;
        var Payment = paymentService.Payment;
        var SecurityCode = paymentService.SecurityCode;
        var Card = paymentService.Card;

        var stepIndex = 0;

        var getSourceCard = function() {
            var cardId = $scope.payment.fromCardId;
            return _.findWhere(data.cards, { cardId: cardId });
        }

        var validateAmount = function (amount) {
            var deferred = $q.defer();
            if (!_.isNumber(amount)) {
                deferred.reject('Cумма платежа должна быть больше нуля.');
            }
            if (!$scope.template) {
                deferred.resolve();
                return null;
            }
            if (amount < 0) {
                deferred.reject('Cумма платежа должна быть больше нуля.');
                return null;
            }
            var currencyIsoName = $scope.template.formTemplate.amount.editor.options.isoName;
            var card = getSourceCard();
            Card.balance({
                cardId: card.cardId,
                currencyIsoName: currencyIsoName
            }).$promise.then(function(data) {
                if (data.requestedBalance < amount) {
                    deferred.reject('Недостаточно средств на счете для осуществления операции.');
                } else {
                    deferred.resolve();
                }
            });
            return deferred.promise;
        }

        var validateFormOnServer = function () {
            var deferred = $q.defer();
            PaymentForm.validate($scope.template.code, $scope.payment.form).then(function (result) {
                if (!result.isValid) {
                    var faults = _.map(result.faults, function (x) {
                        x.propertyName = 'form.' + x.propertyName;
                        return x;
                    });
                    uiTools.validate.setFormErrors($scope.paymentController, faults);
                    deferred.reject();
                } else {
                    $scope.card = _.findWhere($scope.cards, { cardId: $scope.payment.fromCardId });
                    deferred.resolve();
                }
            });
            return deferred.promise;
        };

        var next = function () {
            stepIndex++;
            wizardHandler.wizard('paymentWizard').next();
        };

        $scope.errorMessages = [];
        $scope.cards = data.cards;
        $scope.card = data.cards[0];
        $scope.smsConfirmationEnabled = data.profile.smsConfirmationEnabled;
        $scope.smsCodeSent = false;

        $scope.template = null;

        $scope.paymentController = {};

        var defaults = {
            payment: {
                fromCardId: $scope.cards[0].cardId,
                securityCode: {},
                form: {}
            },
            validators: {
                form: {}
            }
        }

        $scope.payment = angular.copy(defaults.payment);

        $scope.validators = angular.copy(defaults.validators);

        $scope.sendSmsCode = function () {
            function onSuccess(code) {
                $scope.payment.securityCode.id = code.id;
                uiTools.notify({
                    type: 'info',
                    message: 'Введите код подтверждения в поле смс-код'
                });
                $scope.smsCodeSent = true;
            };
            SecurityCode.generate().then(onSuccess);
        };

        $scope.back = function() {
            wizardHandler.wizard('paymentWizard').previous();
            stepIndex--;
        };

        $scope.cancel = function () {
            stepIndex = 0;
            $scope.payment = angular.copy(defaults.payment);
            $scope.validators = angular.copy(defaults.validators);
            wizardHandler.wizard('paymentWizard').goTo(0);
            $scope.smsCodeSent = false;
            $scope.paymentController.resetErrors();
        };

        $scope.selectPaymentCategory = function (category) {
            Payment.getTemplate({ code: category.code }).$promise.then(function (template) {
                $scope.template = template;
                var formDescription = PaymentForm.create(template.formTemplate);
                $scope.payment.templateCode = template.code;
                $scope.payment.form = formDescription.form;
                $scope.validators.form = formDescription.validators;
                $scope.validators.form.amount.custom = validateAmount;
                $scope.paymentController.resetErrors();
                next();
            });
        };

        $scope.continueToApproval = function () {
            uiTools.validate.handleManualValidation($scope.paymentController)
                .then(validateFormOnServer)
                .then(next);
        };

        $scope.canApprove = function () {
            if (!$scope.smsConfirmationEnabled) {
                return true;
            }
            var securityCode = $scope.payment.securityCode;
            if (_.isUndefined(securityCode)) {
                return false;
            }
            var smsCode = securityCode.code;
            return _.isString(smsCode) && smsCode.length === 6;
        };

        $scope.approve = function () {
            function onSuccess() {
                stepIndex = 3;
                wizardHandler.wizard('paymentWizard').goTo(stepIndex);
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
                stepIndex = 4;
                wizardHandler.wizard('paymentWizard').goTo(stepIndex);
            }
            var promise = Payment.create($scope.payment).$promise;
            promise.then(onSuccess, onError);
        };

        var init = function () {
            if (data.cards.length === 0) {
                uiTools.notify({
                    type: 'warning',
                    message: 'Нет карт с которых разрешен платеж.'
                });
                $state.go('customer.cards.list');
            }
            if (data.prototype) {
                $scope.isReplay = true;
                $scope.template = data.prototype.template;
                $scope.payment.form = data.prototype.form;
                $timeout(next, 0);
            }
        };

        init();
    }

})();