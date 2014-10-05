(function () {
    'use strict';

    var app = angular.module('vabank.webapp');
    app.controller('systemLogController', systemLog);
    
    systemLog.$inject = ['$scope', '$modal', 'promiseTracker', 'controlUtil', 'dataUtil', 'notificationService', 'systemLogService', 'data'];
    
    function systemLog($scope, $modal, promiseTracker, controlUtil, dataUtil, notificationService, systemLogService, data) {
        var multiselect = controlUtil.multiselect;
        var LogEntry = systemLogService.LogEntry;

        var createFilter = function() {
            $scope.filter.type.value = multiselect.getSingleItem($scope.lookups.types);
            $scope.filter.level.value = multiselect.getSelectedItems($scope.lookups.levels);
            var filter = dataUtil.filters.combine($scope.filter, dataUtil.filters.logic.And);
            return filter;
        };

        $scope.loading = promiseTracker();

        $scope.filter = LogEntry.defaults.filter;

        $scope.lookups = {
            levels: multiselect.getSelectChoices(data.lookup.levels, {tickMode: 'all'}),
            types: multiselect.getSelectChoices(data.lookup.types, {tickMode: 'first', preItems: [dataUtil.filters.markers.any('Любой')]}),
        };

        $scope.logs = data.logs;

        $scope.displayedLogs = [].concat(data.logs);

        $scope.show = function () {
            var filter = createFilter().toLINQ();
            $scope.logs = LogEntry.query({ filter: filter });
            $scope.loading.addPromise($scope.logs.$promise);
        };

        $scope.canClear = function() {
            return $scope.displayedLogs.length > 0;
        };

        $scope.clear = function() {
            var promise = LogEntry.clear({ ids: _.pluck($scope.displayedLogs, 'eventId') }).$promise;
            $scope.loading.addPromise(promise);
            promise.then(function(response) {
                $scope.logs = [];
                notificationService.notify({                    
                    type: 'success',
                    message: response.message,
                    title: 'Операция завершена успешно'
                });
            });
        };

        $scope.exception = function (log) {
            LogEntry.exception({ id: log.eventId }).$promise.then(function (details) {
                var logDetails = angular.extend({}, log, details);
                $modal.open({
                    templateUrl: '/Client/app/areas/admin/system-log/exception-details.html',
                    controller: 'exceptionDetailsController',
                    resolve: {
                        data: function () { return logDetails; }
                    }
                });
            });
            
        };
    }

})();
