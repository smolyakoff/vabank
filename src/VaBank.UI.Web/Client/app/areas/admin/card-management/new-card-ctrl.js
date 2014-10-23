(function () {
	'use strict';

	angular
        .module('vabank.webapp')
        .controller('newCardController', newCardController);

	newCardController.$inject = ['$scope', '$state', 'uiTools', 'cardManagementService'];

	function newCardController($scope, $state, uiTools, cardManagementService) {

	    var User = cardManagementService.User;
	    var account = angular.copy($scope.account);
       

		var tabOpened = function () {
			//load limits from server here
		};

		$scope.$on('accountTabChanged', function (e, tabName) {
			if (tabName === 'new-card') {
				tabOpened();
			}
		});

		$scope.loading = uiTools.promiseTracker();

	    $scope.users = [];

		$scope.cardVendors = [
		    { id: 'visa', name: 'Visa' },
			{ id: 'mastercard', name: 'MasterCard' }
		];

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

	    $scope.cardForm = {
	    	accountNo: account.accountNo,
			expirationDate: account.expirationDate
	    };
	}
})();
