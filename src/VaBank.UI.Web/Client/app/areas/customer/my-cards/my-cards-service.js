(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('myCardsService', myCardsService);

    myCardsService.$inject = ['$resource', 'dataUtil', 'authService', 'securityCodeService', 'transferService'];

    function myCardsService($resource, dataUtil, authService, securityCodeService, transferService) {

        var getUserId = function() {
            return authService.getUser().id;
        };

        var Card = $resource('/api/cards/:cardId', {cardId: '@cardId'}, {
            query: {
                url: '/api/users/:userId/cards',
                isArray: true,
                params: { userId: getUserId },
            },
            updateSettings : {
                url: '/api/cards/:cardId/settings',
                method: 'PUT',
            },
            block: {
                url: '/api/cards/:cardId/block',
                method: 'POST',
            },
            statement: {
                url: '/api/cards/:cardId/statement',
                method: 'GET'
            }
        });
        Card.queryNotBlocked = function() {
            return Card.query().$promise.then(function(cards) {
                return _.where(cards, { blocked: false });
            });
        };

        var SecurityCode = securityCodeService;

        var Transfer = transferService.Transfer;

        return {
            Card: Card,
            SecurityCode: SecurityCode,
            Transfer: Transfer
        };

    }
})();