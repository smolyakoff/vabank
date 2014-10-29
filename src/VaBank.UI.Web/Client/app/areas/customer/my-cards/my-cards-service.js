(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('myCardsService', myCardsService);

    myCardsService.$inject = ['$resource', 'dataUtil', 'authService'];

    function myCardsService($resource, dataUtil, authService) {

        var getUserId = function() {
            return authService.getUser().id;
        };

        var Card = $resource('/api/cards/:cardId', {}, {
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
            }
        });

        return {
            Card: Card
        };

    }
})();