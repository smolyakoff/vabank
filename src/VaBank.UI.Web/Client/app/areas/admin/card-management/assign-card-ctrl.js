(function () {
	'use strict';

	angular
        .module('vabank.webapp')
        .controller('assignCardController', assignCardController);

	assignCardController.$inject = ['$scope', '$state', 'uiTools'];

	function assignCardController($scope, $state, uiTools) {
		var account = angular.copy($scope.account);

	    $scope.loading = uiTools.promiseTracker();

		var tabOpened = function () {
			//load unassigned cards from server here
		};

		$scope.cards = [
            {
                cardId: 'id',
                cardNo: '1525-1010-1245-1424',
                cardholderFirstName: 'JOHN',
                cardholderLastName: 'DOE',
                cardVendor: {
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

	    $scope.$on('accountTabChanged', function (e, tabName) {
			if (tabName === 'assign-card') {
				tabOpened();
			}
		});
	}
})();
