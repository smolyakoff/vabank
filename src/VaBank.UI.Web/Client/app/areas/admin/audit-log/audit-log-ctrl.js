(function () {
    'use strict';

    var app = angular.module('vabank.webapp');
    app.controller('auditLogController', auditLog);
    
    auditLog.$inject = ['$scope','uiTools', 'auditLogService'];

    function auditLog($scope, uiTools, auditLogService) {
        var multiselect = uiTools.control.multiselect;
        var filters = uiTools.manipulate.filters;
        var LogEntry = auditLogService.LogEntry;
        var User = auditLogService.User;

        var anyUser = { userName: 'Любой', userId: filters.markers.any() };

        $scope.filter = angular.copy(LogEntry.defaults.filter);

        $scope.loading = uiTools.promiseTracker();

        $scope.lookup = {            
          codes: multiselect.getSelectChoices(['LOGIN', 'UPDATE-USER'])
        };

        $scope.users = [];

        $scope.logs = [
            {
                operationId: 'abc',
                startedUtc: new Date(),
                userName: 'terminator',
                applicationId: 'vabank.webapp',
                appActions: [
                    { code: 'LOGIN', description: 'User logged in', jsonData: '{ "abc": "abc"}' },
                    { code: 'LOGIN', description: 'User logged in', jsonData: '{ "abc": "abc"}' },
                    { code: 'LOGIN', description: 'User logged in', jsonData: '{ "abc": "abc"}' }
                ]
            },
            {
                operationId: 'abc',
                startedUtc: new Date(),
                userName: 'terminator',
                applicationId: 'vabank.webapp',
                appActions: [
                    { code: 'LOGIN', description: 'User logged in', jsonData: '{ "abc": "abc"}' },
                ]
            }
        ];

        $scope.displayedLogs = [].concat($scope.logs);

        $scope.formatUser = function (user) {
            var userString = uiTools.format("{0} {1} ({2})", user.firstName, user.lastName, user.email);
            return userString;
        };

        $scope.searchUser = function (searchString) {
            if (!searchString || searchString.length < 2) {
                return;
            }
            var filter = angular.copy(User.defaults.filter);
            _.forEach(filter, function(x) {
                x.value = searchString;
            });
            User.query({
                filter: filters.combine(filter, filters.logic.Or).toLINQ(),
                pageSize: 10000000,
            }).$promise.then(function(page) {
                $scope.users = [anyUser].concat(page.items);
            });
        };

        $scope.show = function() {
            debugger;
        };

    }


})();
