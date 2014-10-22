(function () {
	'use strict';

	angular
        .module('vabank.webapp')
        .controller('newCardController', newCardController);

	newCardController.$inject = ['$scope', '$state', 'uiTools'];

	function newCardController($scope, $state, uiTools) {
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

		$scope.cardVendors = [
		    { id: 'visa', name: 'Visa' },
			{ id: 'mastercard', name: 'MasterCard' }
		];

	    $scope.cardForm = {
	    	accountNo: account.accountNo,
			expirationDate: account.expirationDate
	    };
	}
})();
