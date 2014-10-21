(function () {
    'use strict';

    var app = angular.module('vabank.webapp');
    app.controller('auditLogItemController', auditLogItem);
    
    auditLogItem.$inject = ['$scope', 'uiTools', 'auditLogService'];

    function auditLogItem($scope, uiTools, auditLogService) {
        var item = $scope.log;
        var transition = {
            'app': 'db',
            'db': 'app'
        };

        $scope.view = 'app';
        $scope.detailedLog = {
            dbActions: [
                {
                    tableName: '[Membership].[User]',
                    changes: [
                        {
                            action: 'Insert',
                            timestampUtc: new Date(),
                            values: { id: 213, userName: 'name', age: 15 }
                        },
                        {
                            action: 'Update',
                            timestampUtc: new Date(),
                            values: { id: 213, userName: 'name', age: 15 }
                        },
                    ]
                }
            ]
        };

        $scope.switchView = function () {
            var currentView = $scope.view;
            $scope.view = transition[currentView];

        };
    }

})();
