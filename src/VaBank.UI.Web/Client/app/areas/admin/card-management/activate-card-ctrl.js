(function () {
	'use strict';

	angular
        .module('vabank.webapp')
        .controller('activateCardController', activateCardController);

	activateCardController.$inject = ['$scope', '$state', 'uiTools', 'cardManagementService'];

	function activateCardController($scope, $state, uiTools, cardManagementService) {
	    var account = angular.copy($scope.account);
	    var Card = cardManagementService.Card;
	    var AccountCard = cardManagementService.AccountCard;

	    $scope.loading = uiTools.promiseTracker();

		var loadData = function () {
		    var promise = AccountCard.query({accountNo: account.accountNo, isActive: false}).$promise;
		    $scope.itemLoading.addPromise(promise);
		    promise.then(function(cards) {
		        $scope.cards = cards;
		    });
		};

		$scope.cards = [];

	    $scope.displayedCards = angular.copy($scope.cards);

	    $scope.activate = function(card) {
	        var params = {
	            cardId: card.cardId,
	            isActive: true
	        };
	        var promise = Card.activate(params).$promise;
	        $scope.itemLoading.addPromise(promise);
	        promise.then(function(response) {
	            $scope.cards = _.without($scope.cards, card);
	            uiTools.notify({	                
	                type: 'success',
	                message: response.message
	            });
	        });
	    };

	    $scope.$on('accountTabChanged', function (e, tabName) {
			if (tabName === 'activate-card') {
				loadData();
			}
	    });

	    loadData();
	}
})();
