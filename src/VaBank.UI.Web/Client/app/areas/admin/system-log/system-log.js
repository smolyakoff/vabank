(function () {
    'use strict';

    var app = angular.module('vabank.webapp');
    app.controller('systemLogController', systemLog);
    
    systemLog.$inject = ['$scope'];
    
    function systemLog($scope) {
        $scope.lookups = {
            logLevels: [{name: 'INFO'}, {name: 'WARN'}]
        };

        $scope.from = moment().date(1).startOf('day').utc().toDate();
        $scope.to = moment().endOf('day').utc().toDate();

        $scope.show = function() {

        };
    }

})();
