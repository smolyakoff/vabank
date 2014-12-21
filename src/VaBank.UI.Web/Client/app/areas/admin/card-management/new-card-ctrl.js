(function () {
	'use strict';

	angular
        .module('vabank.webapp')
        .controller('newCardController', newCardController);

	newCardController.$inject = ['$scope', '$state', 'uiTools', 'cardManagementService'];

	function newCardController($scope, $state, uiTools, cardManagementService) {

	    var lookup = angular.copy($scope.lookup);
	    var User = cardManagementService.User;
	    var AccountCard = cardManagementService.AccountCard;
	    var account = angular.copy($scope.account);

	    var defaults = {
	        accountNo: account.accountNo,
	        expirationDateUtc: moment.utc(account.expirationDateUtc).subtract(1, 'month').endOf('month'),
	        userId: account.owner.userId,
	        cardVendorId: lookup.cardVendors[0].id
	    };

		$scope.loading = uiTools.promiseTracker();

	    $scope.users = [account.owner];

	    $scope.cardVendors = lookup.cardVendors;

	    $scope.formController = {};

	    $scope.cardForm = angular.copy(defaults);

	    $scope.validationRules = {
	        userId: { required: true },
            expirationDateUtc: {
                custom: uiTools.validate.createValidator(function (value) {
                    var now = moment();
                    var cardExpires = moment(value);
                    if (cardExpires.isBefore(now)) {
                        return 'Срок действия не может быть раньше текущей даты.';
                    }
                    var accountExpires = moment.utc(account.expirationDateUtc).startOf('month');
                    if (cardExpires.isBefore(accountExpires)) {
                        return null;
                    }
                    return 'Срок действия карты не может быть больше срока действия карт-счета.';
                })
            },
            cardVendorId: { required: true },
            cardholderFirstName: { required: true },
            cardholderLastName: { required: true }
	    };

	    $scope.searchUser = function(searchString) {
	        if (!searchString) {
	            return;
	        }
	        User.search({
	            searchString: searchString,
	            roles: ['Customer']
	        }).then(function(users) {
	            $scope.users = users;
	        });
	    };

	    $scope.formatUser = User.format;

	    $scope.create = function () {
	        var promise = AccountCard.create($scope.cardForm).$promise;
	        return uiTools.validate.handleServerResponse(promise);
	    };

	    $scope.onCreated = function(response) {
	        uiTools.notify({	            
	            type: 'success',
	            message: response.message
	        });
	        $scope.cardForm = angular.copy(defaults);
	        $scope.formController.resetFields();
	        $scope.changeTab('cards');
	    };

	    $scope.$watch('cardForm.expirationDateUtc', function (newVal, oldVal) {
	        if (oldVal === newVal || !moment(newVal).isValid()) {
	            return;
	        }
	        $scope.cardForm.expirationDateUtc = moment(newVal).endOf('month');
	    });

	}
})();
