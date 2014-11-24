(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('transferService', trasferService);

    trasferService.$inject = ['$resource'];

    function trasferService($resource) {

        var Transfer = $resource('/api/transfers/:type', {type: '@type'}, {
            create: {
                method: 'POST',
            },
            lookup: {
                url: '/api/transfers/lookup',
                method: 'GET'
            }
        });

        return {
            Transfer: Transfer
        };
    }
})();