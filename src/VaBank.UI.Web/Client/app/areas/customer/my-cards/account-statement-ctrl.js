(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('accountStatementController', accountStatementController);

    accountStatementController.$inject = ['$scope', 'myCardsService', 'uiTools', 'data'];

    function accountStatementController($scope, myCardsService, uiTools, accounts) {
        
        var CardAccount = myCardsService.CardAccount;

        var dateRange = function (ago, dim, to) {
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
        
        $scope.accounts = accounts;
        $scope.account = { selected: accounts[0] };
        $scope.selectedRange = $scope.ranges[0];

        var initialRange = $scope.selectedRange.range();

        $scope.fromDate = initialRange.from;
        $scope.toDate = initialRange.to;

        $scope.statement = null;
        $scope.displayedTransactions = [];

        $scope.abs = function(num) {
            return Math.abs(num);
        };

        $scope.toLocalDate = function(date) {
            return moment.utc(date).local();
        };

        $scope.rangeSelected = function (item) {
            $scope.selectedRange = item;
            if (!item.isCustom) {
                var range = item.range();
                $scope.fromDate = range.from;
                $scope.toDate = range.to;
            }
        };

        $scope.canLoadStatement = function() {
            return moment($scope.fromDate).isBefore($scope.toDate);
        };

        $scope.loadStatement = function () {
            if (moment($scope.fromDate).isAfter($scope.toDate)) {
                uiTools.notify({
                    type: 'error',
                    message: 'Период для формирования выписки задан неверно.'
                });
                return;
            }
            var promise = CardAccount.statement({
                accountNo: $scope.account.selected.accountNo,
                from: $scope.fromDate.toJSON(),
                to: $scope.toDate.toJSON()
            }).$promise.then(function (statement) {
                $scope.statement = statement;
                $scope.displayedTransactions = angular.copy(statement.transactions);
            });
            $scope.loading.addPromise(promise);
        };
    }
})();