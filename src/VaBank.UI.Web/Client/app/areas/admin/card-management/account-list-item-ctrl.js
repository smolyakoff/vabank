(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('accountListItemController', accountListItemController);

    accountListItemController.$inject = ['$scope', '$state', 'uiTools']; 

    function accountListItemController($scope, $state, uiTools) {
        
        $scope.itemLoading = uiTools.promiseTracker();

        $scope.changeTab = function (tabName) {
            var changed = $scope.tab !== tabName;
            $scope.tab = tabName;
            if (changed) {
                $scope.$broadcast('accountTabChanged', tabName);
            }
        };

        $scope.$watch('account.isSelected', function (selected) {
            if (selected) {
                $scope.changeTab('cards');
            }
        });
    }
})();
