(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('cardListController', cardListController);

    cardListController.$inject = ['$scope', '$state', 'uiTools'];

    function cardListController($scope, $state, uiTools) {
        var account = $scope.account;

        var tabOpened = function() {
            //load cards from server here
        };

        $scope.cards = [
            {
                cardId: 'id',
                cardNo: '1525-1010-1245-1424',
                cardholderFirstName: 'JOHN',
                cardholderLastName: 'DOE',
                cardVendor : {
                    name: 'Visa',
                    imageUrl: '/Client/images/icons/visa-curved-128px.png'
                }
            },
            {
                cardId: 'id',
                cardNo: '6666-1010-1214-1424',
                cardholderFirstName: 'JOHN',
                cardholderLastName: 'DOE',
                cardVendor: {
                    name: 'Visa',
                    imageUrl: '/Client/images/icons/visa-curved-128px.png'
                }
            }
        ];

        $scope.displayedCards = angular.copy($scope.cards);

        $scope.$on('accountTabChanged', function(e, tabName) {
            if (tabName === 'cards') {
                tabOpened();
            }
        });
    }
})();
