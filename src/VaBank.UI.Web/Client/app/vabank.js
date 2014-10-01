(function () {
    'use strict';
    var webapp = angular.module('vabank.webapp', [
        'ngResource',
        'ui.router',
        'ui.bootstrap',
        'ui.bootstrap.datetimepicker',
        'ui.dateTimeInput',
        'cfp.loadingBar',
        'multi-select',
        'smart-table',
        'formFor', 
        'formFor.bootstrapTemplates',
        
        'vabank.ui'
    ]);

    webapp.run(['routingInterceptor', main]);
    
    function main(routingInterceptor) {
        console.log('Application is running');
        routingInterceptor.initialize();
    }

})();