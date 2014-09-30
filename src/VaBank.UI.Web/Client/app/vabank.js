(function () {
    'use strict';
    var webapp = angular.module('vabank.webapp', [
        'ngResource',
        'ui.router',
        'ui.bootstrap',
        'ui.bootstrap.datetimepicker',
        'ui.dateTimeInput',
        'cfp.loadingBar',
        'datePicker',
        'multi-select',
        'smart-table',
        
        'vabank.ui'
    ]);

    webapp.run(['routingInterceptor', main]);
    
    function main(routingInterceptor) {
        console.log('Application is running');
        routingInterceptor.initialize();
    }

})();