(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('cardListController', cardListController);

    cardListController.$inject = ['$scope', '$state', 'uiTools', 'cardManagementService'];

    function cardListController($scope, $state, uiTools, cardManagementService) {
        var account = $scope.account;
        var AccountCard = cardManagementService.AccountCard;

        var loadData = function() {
            var promise = AccountCard.query({ accountNo: account.accountNo }).$promise;
            $scope.itemLoading.addPromise(promise);
            promise.then(function (cards) {
                $scope.cards = cards;
            });
        };

        $scope.cards = [];

        $scope.displayedCards = angular.copy($scope.cards);

        $scope.unassign = function (card) {
            var assignment = {                
                cardId: card.cardId,
                accountNo: account.accountNo,
                assigned: false
            };
            var promise = AccountCard.assign(assignment).$promise;
            $scope.itemLoading.addPromise(promise);
            promise.then(function(response) {
                $scope.cards = _.without($scope.cards, card);
                uiTools.notify({                    
                    type: 'success',
                    message: response.message
                });
            });

        };

        $scope.$on('accountTabChanged', function (e, tabName) {
            if (tabName === 'cards') {
                loadData();
            }
        });

        loadData();
    }
})();
