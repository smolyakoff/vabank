(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('cardsService', cardsService);

    cardsService.$inject = ['$resource', 'dataUtil'];

    function cardsService($resource, dataUtil) {
        return {
            dateTime: {
                from: {
                    value: moment().date(1).utc().startOf('day').toDate(),
                },
                to: {
                    value: moment().utc().startOf('day').add(1, 'd').toDate(),
                }
            }
        };
    }
})();