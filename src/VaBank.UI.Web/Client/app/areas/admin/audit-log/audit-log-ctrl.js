(function () {
    'use strict';

    var app = angular.module('vabank.webapp');
    app.controller('auditLogController', auditLog);
    
    auditLog.$inject = ['$scope','uiTools', 'auditLogService', 'data'];

    function auditLog($scope, uiTools, auditLogService, data) {
        var multiselect = uiTools.control.multiselect;
        var filters = uiTools.manipulate.filters;
        var LogEntry = auditLogService.LogEntry;
        var User = auditLogService.User;

        var dummyUsers = [
            { userName: 'Любой', userId: filters.markers.any() },
            { userName: 'Аноним', userId: null }
        ];

        $scope.loading = uiTools.promiseTracker();

        $scope.lookup = {            
          codes: multiselect.getSelectChoices(data.lookup.codes)
        };

        $scope.filter = LogEntry.defaults.filterValues();

        $scope.users = [];

        $scope.logs = data.logs;

        $scope.displayedLogs = [].concat($scope.logs);

        $scope.toLocalDate = function(utcDate) {
            return moment.utc(utcDate).toDate();
        };

        $scope.formatUser = User.format;

        $scope.searchUser = function (searchString) {
            if (!searchString || searchString.length < 2) {
                return;
            }
            User.search({searchString: searchString}).then(function(users) {
                $scope.users = dummyUsers.concat(users);
            });
        };

        $scope.show = function () {
            $scope.filter.code = multiselect.getSelectedItems($scope.lookup.codes);
            var promise = LogEntry.query($scope.filter).$promise;
            $scope.loading.addPromise(promise);
            promise.then(function(logs) {
                $scope.logs = logs;
            });
        };
    }


})();
