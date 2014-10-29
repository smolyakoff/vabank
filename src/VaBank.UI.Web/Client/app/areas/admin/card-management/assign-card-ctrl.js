(function () {
	'use strict';

	angular
        .module('vabank.webapp')
        .controller('assignCardController', assignCardController);

	assignCardController.$inject = ['$scope', '$state', 'uiTools', 'cardManagementService'];

	function assignCardController($scope, $state, uiTools, cardManagementService) {
	    var account = angular.copy($scope.account);
	    var Card = cardManagementService.Card;
	    var AccountCard = cardManagementService.AccountCard;

	    $scope.loading = uiTools.promiseTracker();

		var loadData = function () {
		    var promise = Card.queryUnassigned().$promise;
		    $scope.itemLoading.addPromise(promise);
		    promise.then(function(cards) {
		        $scope.cards = cards;
		    });
		};

		$scope.cards = [];

	    $scope.displayedCards = angular.copy($scope.cards);

	    $scope.assign = function(card) {
	        var assignment = {
	            cardId: card.cardId,
	            accountNo: account.accountNo,
	            assigned: true
	        };
	        var promise = AccountCard.assign(assignment).$promise;
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
			if (tabName === 'assign-card') {
				loadData();
			}
	    });

	    loadData();
	}
})();
