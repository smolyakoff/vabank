(function () {
    'use strict';

    var app = angular.module('vabank.webapp');
    app.controller('auditLogController', auditLog);
    
    auditLog.$inject = ['$scope', 'uiTools', 'auditLogService'];

    function auditLog($scope, uiTools, auditLogService) {
        var multiselect = uiTools.control.multiselect;
        var LogEntry = auditLogService.LogEntry;

        $scope.filter = angular.copy(LogEntry.defaults.filter);

        $scope.loading = uiTools.promiseTracker();

        $scope.lookup = {            
          codes: multiselect.getSelectChoices(['LOGIN', 'UPDATE-USER'])
        };

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

        $scope.show = function() {

        };

    }


})();
