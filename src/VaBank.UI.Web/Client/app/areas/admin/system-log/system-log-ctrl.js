(function () {
    'use strict';

    var app = angular.module('vabank.webapp');
    app.controller('systemLogController', systemLog);
    
    systemLog.$inject = ['$scope', 'controlUtil', 'dataUtil', 'systemLogService', 'data'];
    
    function systemLog($scope, controlUtil, dataUtil, systemLogService, data) {
        var multiselect = controlUtil.multiselect;

        var createFilter = function() {
            var selectedTypes = multiselect.getSelectedItems($scope.lookups.types);
            $scope.typeFilter.value = selectedTypes.length === 0 ? null : selectedTypes[0];
            $scope.levelFilter.value = multiselect.getSelectedItems($scope.lookups.levels);
            var filter = dataUtil.filters.combine([
                $scope.fromFilter,
                $scope.toFilter,
                $scope.levelFilter
            ], dataUtil.filters.logic.And);
            return filter;
        };

        $scope.lookups = {
            levels: multiselect.getSelectChoices(data.lookup.levels, {tickMode: 'all'}),
            types: multiselect.getSelectChoices(data.lookup.types, {tickMode: 'first'}),
        };

        $scope.fromFilter = {
            propertyName: 'timestampUtc',
            operator: dataUtil.filters.operator.GreaterThan,
            value: moment().date(1).startOf('day').utc().toDate(),
        };

        $scope.toFilter = {
            propertyName: 'timestampUtc',
            operator: dataUtil.filters.operator.LessThan,
            value: moment().startOf('day').utc().toDate(),
        };

        $scope.typeFilter = {
            propertyName: 'type',
            operator: dataUtil.filters.operator.Equal,
            value: null
        };

        $scope.levelFilter = {
            propertyName: 'level',
            operator: dataUtil.filters.operator.In,
            value: data.lookup.levels
        };

        $scope.logs = data.logs;

        $scope.show = function () {
            var filter = createFilter().toLINQ();
            $scope.logs = systemLogService.LogEntry.query({ filter: filter});
        };

        $scope.clear = function() {
            var filter = createFilter().toObject();
            $scope.logs = systemLogService.LogEntry.clear({filter: filter});
        };


    }

})();
