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

        $scope.formatUser = User.format;

        $scope.searchUser = function (searchString) {
            if (!searchString || searchString.length < 2) {
                return;
            }
            User.search({searchString: searchString}).then(function(users) {
                $scope.users = [anyUser].concat(users);
            });
        };

        $scope.show = function() {
            debugger;
        };
    }


})();
