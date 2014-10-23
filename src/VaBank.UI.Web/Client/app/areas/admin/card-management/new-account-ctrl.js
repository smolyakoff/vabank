(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('newAccountController', newAccountController);

    newAccountController.$inject = ['$scope', '$state', 'uiTools', 'cardManagementService'];

    function newAccountController($scope, $state, uiTools, cardManagementService) {

        var User = cardManagementService.User;

        $scope.cardAccountForm = {
            initialBalance: 0
        };

        $scope.validationRules = {
            userId: { required: true }
        };

        $scope.formatUser = User.format;
        $scope.searchUser = function(searchString) {
            if (!searchString) {
                return;
            }
            User.search({
                searchString: searchString,
                roles: ['Customer']
            }).then(function (users) {
                $scope.users = users;
            });
        };
    }
})();
