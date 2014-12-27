(function () {
    'use strict';

    angular.module('vabank.webapp').controller('paymentsTreeController', paymentsTreeController);

    paymentsTreeController.$inject = ['$scope'];

    function paymentsTreeController($scope) {

        var N = 2;

        $scope.tree = $scope.tree;

        $scope.selected = [];

        $scope.start = 0;
       
        $scope.choose = function (item, column) {
            if ($scope.isLeaf(item)) {
                $scope.selectPaymentCategory(item);
                return;
            }
            $scope.selected[item.level - 1] = item;
            $scope.selected.splice(item.level, Number.MAX_VALUE);
            if (column === 0) {
                $scope.start = 0;
            } else if (column === N) {
                $scope.start++;
            }
        };

        $scope.up = function() {
            $scope.start--;
        };

        $scope.isSelected = function (item) {
            return _.indexOf($scope.selected, item) >= 0;
        };

        $scope.isLeaf = function(item) {
            return (!_.isArray(item.children)) ||
                   (_.isArray(item.children) && item.children.length === 0);
        };

        $scope.reset = function() {
            $scope.selected = [];
            $scope.start = 0;
        };
    }

})();