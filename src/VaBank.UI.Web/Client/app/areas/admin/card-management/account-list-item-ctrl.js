(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('accountListItemController', accountListItemController);

    accountListItemController.$inject = ['$scope', '$state', 'uiTools']; 

    function accountListItemController($scope, $state, uiTools) {

        var init = function() {
            $scope.changeTab('cards');
        };

        $scope.changeTab = function(tabName) {
            var changed = $scope.tab !== tabName;
            var selected = $scope.account.isSelected;
            $scope.tab = tabName;
            if (changed && selected) {
                $scope.$broadcast('accountTabChanged', tabName);
            }
        };

        init();
    }
})();
