(function() {
    'use strict';

    var app = angular.module('vabank.webapp');
    app.controller('userUnlockController', userUnlock);

    userUnlock.$inject = ['$scope', 'uiTools', 'userManagementService', 'data'];

    function userUnlock($scope, uiTools, userManager, data) {
        $scope.user = data.user;
        $scope.profile = data.profile;

        $scope.ok = function() {
            userManager.User.unlock(data.user).$promise.then(function(result) {
                uiTools.notify({
                    type: 'success',
                    message: result.message,
                });
                $scope.$close();
            });
        };
    }
})();