(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('cabinetService', myCardsService);

    myCardsService.$inject = ['$resource', 'dataUtil', 'authService', 'securityCodeService'];

    function myCardsService($resource, dataUtil, authService, securityCodeService) {

        var getUserId = function () {
            return authService.getUser().id;
        };

        var Card = $resource('/api/cards/:cardId', {}, {
            query: {
                url: '/api/users/:userId/cards',
                isArray: true,
                params: { userId: getUserId },
            },
            updateSettings: {
                url: '/api/cards/:cardId/settings',
                method: 'PUT',
            }
        });

        var SecurityCode = securityCodeService;

        return {
            Card: Card,
            SecurityCode: SecurityCode
        };

    }
})();