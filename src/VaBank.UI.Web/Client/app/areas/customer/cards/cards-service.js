(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('cardsService', cardsService);

    cardsService.$inject = ['$resource'];

    function cardsService($resource) {
        return {

        };
    }
})();