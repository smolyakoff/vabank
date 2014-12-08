(function () {
    'use strict';

    angular.module('vabank.webapp').controller('paymentController', paymentController);

    paymentController.$inject = ['$scope', 'uiTools', 'WizardHandler', 'paymentService', 'data'];

    function paymentController($scope, uiTools, wizardHandler, paymentService, data) {

        var PaymentForm = paymentService.PaymentForm;

        var init = function() {
            if (data.cards.length === 0) {
                uiTools.notify({
                    type: 'warning',
                    message: 'Нет карт с которых разрешен платеж.'
                });
                $state.go('customer.cards.list');
            }
        };

        var next = function() {
            wizardHandler.wizard('paymentWizard').next();
        };
        
        
        $scope.cards = data.cards;
        $scope.card = data.cards[0];
        $scope.smsConfirmationEnabled = data.profile.smsConfirmationEnabled;
        $scope.smsCodeSent = false;
        
        $scope.template = {
            code: 'PAYMENT-CELL-VELCOM-PHONENO',
            form: {
                "amount": {
                    "value": 0,
                    "editor": {
                        "name": "currency",
                        "options": {
                            "label": "Сумма платежа",
                            "required": true,
                            "symbol": "Br",
                            "precision": 0
                        }
                    }
                },
                "phoneNo": {
                    "value": "",
                    "editor": {
                        "name": "masked-text",
                        "options": {
                            "label": "Номер телефона",
                            "required": true,
                            "mask": "99 9999999",
                            "placeholder": "** *******",
                            "help": "Введите 9 цифр номера телефона в формате: 29ххххххх, 44ххххххх, 25ххххххх, 33ххххххх"
                        }
                    }
                }
            }
        };

        $scope.paymentController = {};

        $scope.payment = {
            fromCardId: $scope.cards[0].cardId,
            form: {
                phoneNo: null,
                amount: 0
            }
        };

        $scope.validators = {
            amount: { custom: angular.noop },
            phoneNo: { custom: angular.noop }
        };

        $scope.back = function() {
            wizardHandler.wizard('paymentWizard').previous();
        };

        $scope.selectTemplate = function(template) {
            //$scope.template = template;
            //$scope.template.templateUrl = '/Client/app/areas/customer/payments/templates/' +
            //    template.code.toLowerCase() + '.html';
            next();
        };

        $scope.continueToApproval = function () {
            PaymentForm.validate($scope.template.code, $scope.payment.form).then(function(result) {
                if (!result.isValid) {
                    var faults = _.map(result.faults, function(x) {
                        x.propertyName = 'form.' + x.propertyName;
                        return x;
                    });
                    uiTools.validate.setFormErrors($scope.paymentController, faults);
                } else {
                    $scope.card = _.findWhere($scope.cards, { cardId: $scope.payment.fromCardId });
                    next();
                }
            });
        };

        $scope.approve = function() {

        };

        init();
    }

})();