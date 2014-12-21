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
            currencyISOName: data.lookup.currencies[0].isoName,
            cardExpirationDateUtc: moment().add(3, 'years').endOf('month').toDate(),
            accountExpirationDateUtc: moment().add(3, 'years').add(1, 'month').toDate()
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
                custom: uiTools.validate.createValidator(function (value) {
                    if(moment(value).isBefore(moment())) {
                        return 'Неверный срок действия счета.';
                    }
                    return null;
                })
            },
            cardExpirationDateUtc: {
                required: true,
                custom: uiTools.validate.createValidator(function (value, model) {
                    var cardExpires = moment(value);
                    var accountExpires = moment.utc(model.accountExpirationDateUtc).startOf('month');
                    if (cardExpires.isBefore(accountExpires)) {
                        return null;
                    }
                    return 'Срок действия карты не может быть больше срока действия карт-счета.';
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

        $scope.$watch('cardAccountForm.cardExpirationDateUtc', function (newVal, oldVal) {
            if (oldVal === newVal) {
                return;
            }
            $scope.cardAccountForm.cardExpirationDateUtc = moment(newVal).endOf('month');
        });
    }
})();
