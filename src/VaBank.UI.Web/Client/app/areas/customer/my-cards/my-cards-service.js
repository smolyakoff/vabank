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

        var Card = $resource('/api/users/:userId/cards', { userId: getUserId }, {
            
        });

        return {
            Card: Card
        };

    }
})();