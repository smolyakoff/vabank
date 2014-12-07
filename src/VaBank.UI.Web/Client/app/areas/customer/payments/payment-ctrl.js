(function () {
    'use strict';

    angular.module('vabank.webapp').controller('paymentController', paymentController);

    paymentController.$inject = ['$scope', 'uiTools', 'WizardHandler', 'data'];

    function paymentController($scope, uiTools, wizardHandler, data) {

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
        $scope.template = null;
        $scope.smsConfirmationEnabled = data.profile.smsConfirmationEnabled;
        $scope.smsCodeSent = false;

        $scope.payment = {
            fromCardId: $scope.cards[0].cardId,
            form: {
                amount: { name: 'Сумма платежа', value: 0 },
            }
        };

        $scope.validators = {
            form: {
                amount: {
                    value: { custom: function (a){} }
                }
            }
        };

        $scope.back = function() {
            wizardHandler.wizard('paymentWizard').previous();
        };

        $scope.selectTemplate = function(template) {
            $scope.template = template;
            $scope.template.templateUrl = '/Client/app/areas/customer/payments/templates/' +
                template.code.toLowerCase() + '.html';
            next();
        };

        $scope.continue = function () {
            $scope.card = _.findWhere($scope.cards, { cardId: $scope.payment.fromCardId });
            next();
        };

        $scope.approve = function() {

        };

        init();
    }

})();