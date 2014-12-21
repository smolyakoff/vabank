(function () {
    'use strict';

    angular.module('vabank.webapp').controller('dashboardController', dashboardController);

    dashboardController.$inject = ['$scope', 'uiTools', 'systemStatsService'];
    function dashboardController($scope, uiTools, systemStatsService) {

        var SystemStats = systemStatsService.SystemStats;

        $scope.loading = {
            overall: uiTools.promiseTracker(),
            transactions: uiTools.promiseTracker()
        };

        $scope.overall = {};

        $scope.transactions = {
            data: {
                cols: [
                    { id: 'date', label: 'Дата', type: 'date' },
                    { id: 'transactionsCount', label: 'Обработано транзакций', type: 'number' }
                ],
                rows: []
            },
            type: 'LineChart',
            options: {
                legend: 'none',
                backgroundColor: 'transparent',
                hAxis: {
                    format: 'dd.MM'
                },
                vAxis: {
                    minValue: 0,
                    viewWindow: {
                        min: 0
                    }
                },
                curveType: 'function'
            }
        };

        var init = function () {
            $scope.loading.overall.addPromise(SystemStats.query({ type: 'info' }).then(function(stats) {
                $scope.overall = stats;
            }));
            $scope.loading.transactions.addPromise(SystemStats.query({ type: 'transactions' }).then(function(stats) {
                $scope.transactions.data.rows = _.chain(stats).map(function(x) {
                    return {c: [{v: moment.utc(x.date).toDate()}, {v: x.transactionsCount}]}
                }).value();
            }));
        };

        init();
    }

})();