(function() {
    'use strict';

    angular.module('vabank.webapp').controller('paymentsArchiveController', paymentsArchiveController);

    paymentsArchiveController.$inject = ['$scope', '$state', '$modal', 'uiTools', 'paymentService', 'data'];
    function paymentsArchiveController($scope, $state, $modal, uiTools, paymentService, data) {

        var Payment = paymentService.Payment;
        var Filters = uiTools.manipulate.filters;

        var dateRange = function(ago, dim, to) {
            return function() {
                return {
                    from: moment().subtract(ago, dim).startOf(dim),
                    to: moment().add(to, 'day').startOf('day')
                };
            };
        };

        $scope.loading = uiTools.promiseTracker();

        $scope.ranges = [
            { name: 'По указанным датам', range: dateRange(0, 'month', 1), isCustom: true },
            { name: 'Сегодня', range: dateRange(0, 'day', 1) },
            { name: 'Вчера', range: dateRange(1, 'day', 0) },
            { name: 'За последние 7 дней', range: dateRange(7, 'day', 1) },
            { name: 'За последний месяц', range: dateRange(0, 'month', 1) }
        ];
        $scope.selectedRange = $scope.ranges[0];
        var initialRange = $scope.selectedRange.range();
        $scope.rangeSelected = function(item) {
            $scope.selectedRange = item;
            if (!item.isCustom) {
                var range = item.range();
                $scope.filters.from = range.from;
                $scope.filters.to = range.to;
            }
        };
        $scope.paymentNames = _.chain(data)
            .map(function (x) { return { code: x.paymentCode, name: x.paymentName } })
            .unique(false, function (x){return x.code})
            .value();
        $scope.paymentNames.unshift({
            name: 'Все',
            code: uiTools.manipulate.filters.markers.any('Любой')
        });
        $scope.amountOperators = [
            { name: 'Больше', value: '>' },
            { name: 'Меньше', value: '<' }
        ];

        $scope.filters = {
            from: initialRange.from,
            to: initialRange.to,
            paymentCode: $scope.paymentNames[0].code,
            amountOperator: '>',
            amount: 0
        };

        $scope.payments = data;
        $scope.displayedPayments = [].concat(data);

        $scope.show = function () {
            var filter = Filters.combine({
                from: {
                    propertyName: 'dateUtc',
                    operator: '>=',
                    value: $scope.filters.from,
                    propertyType: 'datetime'
                },
                to: {
                    propertyName: 'dateUtc',
                    operator: '<=',
                    value: $scope.filters.to,
                    propertyType: 'datetime'
                },
                paymentCode: {
                    propertyName: 'paymentCode',
                    operator: '==',
                    value: $scope.filters.paymentCode,
                    propertyType: 'string'
                },
                amount: {
                    propertyName: 'amount',
                    operator: $scope.filters.amountOperator,
                    value: $scope.filters.amount,
                    propertyType: 'decimal'
                }
            }, 'and').toLINQ();
            var promise = Payment.query({ filter: filter }).$promise.then(function (payments) {
                $scope.payments = payments;
                $scope.displayedPayments = payments;
            });
            $scope.loading.addPromise(promise);
        }

        $scope.details = function (payment) {
            $modal.open({
                templateUrl: '/Client/app/areas/customer/payments/payment-details.html',
                controller: 'paymentDetailsController',
                resolve: {
                    data: function() {
                        return Payment.get({ operationId: payment.operationId }).$promise;
                    }
                }
            });            
        }

    }
})();