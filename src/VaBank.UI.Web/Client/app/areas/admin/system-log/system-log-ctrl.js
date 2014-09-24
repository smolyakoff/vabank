(function () {
    'use strict';

    var app = angular.module('vabank.webapp');
    app.controller('systemLogController', systemLog);
    
    systemLog.$inject = ['$scope', 'controlUtil', 'data'];
    
    function systemLog($scope, controlUtil, data) {
        $scope.lookups = {
            levels: controlUtil.multiselect.getSelectChoices(data.lookup.levels, {tickMode: 'all'}),
            types: controlUtil.multiselect.getSelectChoices(data.lookup.types),
        };

        $scope.from = moment().date(1).startOf('day').utc().toDate();
        $scope.to = moment().endOf('day').utc().toDate();
        $scope.logs = data.logs;

        $scope.show = function() {

        };
    }

})();
