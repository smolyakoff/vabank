(function () {
	'use strict';

	angular
        .module('vabank.webapp')
        .controller('newCardController', newCardController);

	newCardController.$inject = ['$scope', '$state', 'uiTools', 'cardManagementService'];

	function newCardController($scope, $state, uiTools, cardManagementService) {

	    var User = cardManagementService.User;
	    var account = angular.copy($scope.account);
      

		$scope.$on('accountTabChanged', function (e, tabName) {
			if (tabName === 'new-card') {
				
			}
		});

		$scope.loading = uiTools.promiseTracker();

	    $scope.users = [account.owner];

		$scope.cardVendors = [
		    { id: 'visa', name: 'Visa' },
			{ id: 'mastercard', name: 'MasterCard' }
		];

		$scope.cardForm = {
		    accountNo: account.accountNo,
		    expirationDate: account.expirationDate,
		    userId: account.owner.userId,
            cardVendorId: $scope.cardVendors[0].id
		};

	    $scope.validationRules = {
	        userId: { required: true },
            expirationDate: {
                custom: uiTools.validate.createValidator(function (value) {
                   if (moment(value).isAfter(account.expirationDate)) {
                       return 'Срок действия карты не может быть больше срока действия карт-счета.';
                   }
                   return null;
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
	}
})();
