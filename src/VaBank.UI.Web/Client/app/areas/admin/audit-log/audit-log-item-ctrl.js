(function () {
    'use strict';

    var app = angular.module('vabank.webapp');
    app.controller('auditLogItemController', auditLogItem);
    
    auditLogItem.$inject = ['$scope', 'uiTools', 'auditLogService'];

    function auditLogItem($scope, uiTools, auditLogService) {
        var log = $scope.log;
        var LogEntry = auditLogService.LogEntry;

        $scope.view = 'app';

        $scope.itemLoading = uiTools.promiseTracker();

        $scope.detailedLog = null;

        $scope.switchView = function () {
            var previousView = $scope.view;
            $scope.view = previousView === 'app' ? 'db' : 'app';

            if ($scope.view == 'db' && $scope.detailedLog == null) {
                var promise = LogEntry.get({ operationId: log.operationId }).$promise;
                $scope.itemLoading.addPromise(promise);
                promise.then(function(detailedLog) {
                    $scope.detailedLog = detailedLog;
                });
            }
        };
    }

})();
