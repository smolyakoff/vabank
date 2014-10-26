(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('cardAccountListController', cardAccountListController);

    cardAccountListController.$inject = ['$scope', '$state', 'uiTools', 'cardManagementService', 'data']; 

    function cardAccountListController($scope, $state, uiTools, cardManagementService, data) {

        var CardAccount = cardManagementService.CardAccount;
        var queryService = uiTools.manipulate.query;
        

        var lastParams = { searchString: '', pageNumber: 1 };
        $scope.loading = uiTools.promiseTracker();

        $scope.search = '';

        $scope.accounts = [];

        $scope.lookup = data.lookup;

        $scope.query = function(tableState) {
            var params = angular.extend(
                {},
                queryService.fromStTable(tableState),
                lastParams);
            var promise = CardAccount.search(params);
            $scope.loading.addPromise(promise);
            promise.then(function (page) {
                tableState.pagination.start = page.skip;
                tableState.pagination.numberOfPages = page.totalPages;
                $scope.accounts = page.items;
            });
        };
        
        $scope.show = function () {
            lastParams = {
                pageNumber: 1,
                searchString: $scope.search
            };
        };

    }
})();
