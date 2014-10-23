(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('cardAccountListController', cardAccountListController);

    cardAccountListController.$inject = ['$scope', '$state', 'uiTools']; 

    function cardAccountListController($scope, $state, uiTools) {

        $scope.accounts = [
            {
                accountNo: '15565646566',
                cardNo: '4556-4548-5465-56454',
                owner: {
                    userId: 'adfasd-asdf',
                    firstName: 'Большой',
                    lastName: 'Джон',
                    userName: 'bigjohn'
                },
                balance: 500,
                currency: {
                    isoName: 'USD'
                },
                expirationDate: new Date()
            },
            {
                accountNo: '15565646566',
                cardNo: '4556-4548-5465-56454',
                owner: {
                    userId: 'adfasd-asdf',
                    firstName: 'Большой',
                    lastName: 'Джон',
                    userName: 'bigjohn'
                },
                balance: 500,
                currency: {
                    isoName: 'USD'
                },
                expirationDate: new Date()
            }
        ];

    }
})();
