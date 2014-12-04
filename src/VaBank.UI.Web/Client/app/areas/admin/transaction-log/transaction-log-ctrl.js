(function () {
    'use strict';

    var app = angular.module('vabank.webapp');
    app.controller('transactionLogController', transactionLog);

    transactionLog.$inject = ['$scope', '$modal', '$q', 'uiTools', 'transactionLogService', 'userManagementService', 'data'];

    function transactionLog($scope, $modal, $q, uiTools, transactionLogService, userService, data) {
        var LogEntry = transactionLogService.LogEntry;
        var CardAccount = transactionLogService.CardAccount;
        var Currency = transactionLogService.Currency;
        var User = userService.User;

        var filters = uiTools.manipulate.filters;
        var multiselect = uiTools.control.multiselect;
        
        var dummyUsers = [
            { userName: 'Любой', userId: filters.markers.any() },
        ];

        $scope.loading = uiTools.promiseTracker();

        $scope.filter = LogEntry.defaults.filterValues();
        $scope.statuses = multiselect.getSelectChoices(LogEntry.statuses());

        $scope.logs = data;

        $scope.displayedLogs = angular.copy($scope.logs);

        $scope.users = [];
        
        $scope.formatUser = User.format;

        $scope.searchAccounts = function(searchString) {
            if (!_.isString(searchString)) {
                return;
            }
            if (searchString.length < 2) {
                return;
            }
            CardAccount.search({ searchString: searchString }).then(function (accounts) {
                var users =_.chain(accounts).groupBy(function(x) {
                    return x.owner.userId;
                }).map(function(v, k) {
                    return _.extend({}, v[0].owner, {                        
                       accounts: _.pluck(v, 'accountNo') 
                    });
                }).value();

                $scope.users = dummyUsers.concat(users);
            });
        };

        $scope.show = function() {
            $scope.filter.status = multiselect.getSelectedItems($scope.statuses);
            var promise = LogEntry.query($scope.filter).$promise;
            $scope.loading.addPromise(promise);
            promise.then(function (logs) {
                $scope.logs = logs;
            });
        };

        $scope.details = function(log) {
            LogEntry.get({ transactionId: log.transactionId }).$promise.then(function(details) {
                $modal.open({
                    templateUrl: '/Client/app/areas/admin/transaction-log/transaction-details.html',
                    controller: 'transactionDetailsController',
                    size: 'lg',
                    resolve: {
                        data: function() {
                            return $q.all({ entry: details, currencies: Currency.query().$promise });
                        }
                    }
                });
            });
        };
    }

})();
