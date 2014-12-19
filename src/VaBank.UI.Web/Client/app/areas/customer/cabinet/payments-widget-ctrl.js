(function() {
    'use strict';

    angular.module('vabank.webapp').controller('paymentsWidgetController', paymentsWidgetController);

    paymentsWidgetController.$inject = ['$scope', '$state', 'uiTools', 'paymentService'];

    function paymentsWidgetController($scope, $state, uiTools, paymentService) {

        var Payment = paymentService.Payment;
        var Card = paymentService.Card;

        $scope.loading = uiTools.promiseTracker();

        $scope.chart = {
            data: {
                cols: [
                    { id: 'categoryName', label: 'Название платежа', type: 'string' },
                    { id: 'amount', label: 'Сумма', type: 'number' }
                ],
                rows: []
            },
            type: 'PieChart',
            options: {
                legend: 'none',
                backgroundColor: 'transparent',
                height: 280,
                chartArea: {
                    height: 250
                },
                is3D: true
            }
        };

        $scope.costs = {};

        $scope.popularPayments = [];

        $scope.pay = function(paymentCode) {
            $state.go('customer.payments.payment', { code: paymentCode });
        }

        var init = function() {
            var mostlyUsed = Payment.getMostlyUsed().$promise.then(function (popular) {
                $scope.popularPayments = _.pluck(popular, 'category');
            });
            $scope.loading.addPromise(mostlyUsed);
            var costsByCategory = Card.query().$promise.then(function chooseRandomCard(cards) {
                var idx = Math.floor(Math.random() * (cards.length - 1));
                return Card.costsByPaymentCategory({ cardId: cards[idx].cardId }).$promise;
            }).then(function (costs) {
                $scope.costs = costs;
                $scope.chart.data.rows = _.map(costs.data, function(item) {
                    return {c: [{v: item.category.displayName}, {v: item.amount}]}
                });
            });
            $scope.loading.addPromise(costsByCategory);
        };

        init();

    }
})();