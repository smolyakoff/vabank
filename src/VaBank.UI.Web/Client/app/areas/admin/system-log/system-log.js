(function () {
    'use strict';

    var app = angular.module('vabank.webapp');
    app.controller('systemLogController', systemLog);
    
    systemLog.$inject = ['$scope'];
    
    function systemLog($scope) {
        $scope.from = new Date();
    }

})();
