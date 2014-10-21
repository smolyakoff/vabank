(function () {
    'use strict';

    var app = angular.module('vabank.webapp');
    app.controller('auditLogItemController', auditLogItem);
    
    auditLogItem.$inject = ['$scope', 'uiTools', 'auditLogService'];

    function auditLogItem($scope, uiTools, auditLogService) {
        var item = $scope.log;
        
    }


})();
