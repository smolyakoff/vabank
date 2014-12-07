(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('newAccountController', newAccountController);

    newAccountController.$inject = ['$scope', '$state', 'uiTools', 'cardManagementService', 'data'];

    function newAccountController($scope, $state, uiTools, cardManagementService, data) {

        var User = cardManagementService.User;
        var CardAccount = cardManagementService.CardAccount;

        $scope.lookup = data.lookup;

        $scope.cardAccountForm = {
            initialBalance: 0,
            cardVendorId: data.lookup.cardVendors[0].id,
            currencyISOName: data.lookup.currencies[0].isoName
        };

        $scope.validationRules = {
            userId: {
                required: true
            },
            cardholderFirstName: {
                required: true
            },
            accountExpirationDateUtc: {
                required: true,
                custom: uiTools.validate.createValidator(function (value, model) {
                    if(moment(value).isBefore(Date.now())) {
                        return 'Неверный срок действия счета.';
                    }
                    return null;
                })
            },
            cardExpirationDateUtc: {
                required: true,
                custom: uiTools.validate.createValidator(function (value, model) {
                    if (moment(value).isBefore(Date.now())) {
                        return 'Неверный срок действия карты.';
                    }
                    if (moment(value).isBefore(model.accountExpirationDate)) {
                        return 'Срок действия карты не может быть больше срока действия карт-счета.';
                    }
                    return null;
                })
            },
            cardholderLastName: {
                required: true
            },
            initialBalance: {
                custom: uiTools.validate.createValidator(function (value, model) {
                    if (value < 0) {
                        return 'Баланс не должен быть меньше нуля.';
                    }
                    return null;
                })
            }
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

        $scope.create = function () {
            var params = angular.copy($scope.cardAccountForm);
            params.accountExpirationDateUtc = moment(params.accountExpirationDateUtc).add(-moment().zone(), 'm');
            params.cardExpirationDateUtc = moment(params.cardExpirationDateUtc).add(-moment().zone(), 'm');
            var promise = CardAccount.create(params).$promise;
            return uiTools.validate.handleServerResponse(promise);
        };

        $scope.onCreated = function (response) {
            uiTools.notify({                
                type: 'success',
                message: response.message
            });
            $state.go('admin.cardManagement.list');
        };
    }
})();
