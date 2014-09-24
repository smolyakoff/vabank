(function () {
    'use strict';
    var webapp = angular.module('vabank.webapp', [
        'ngResource',
        'ui.router',
        'ui.bootstrap',
        'cfp.loadingBar',
        'datePicker',
        'multi-select',
        'smart-table'
    ]);

    webapp.run(['routingInterceptor', main]);
    
    function main(routingInterceptor) {
        console.log('Application is running');
        routingInterceptor.initialize();
    }

})();