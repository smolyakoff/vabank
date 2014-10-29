(function () {
    'use strict';
    var webapp = angular.module('vabank.webapp', [
        'ngResource',
        'ngSanitize',
        'ui.router',
        'LocalStorageModule',
        'ui.bootstrap',
        'ui.bootstrap.datetimepicker',
        'ui.dateTimeInput',
        'ui.select',
        'cfp.loadingBar',
        'ajoslin.promise-tracker',
        'angularSpinner',
        'multi-select',
        'smart-table',
        'formFor', 
        'formFor.bootstrapTemplates',
        
        'vabank.ui',
        'vabank.auth'
    ]);

    webapp.run(['routingInterceptor', 'uiConfig', main]);
    
    function main(routingInterceptor, uiConfig) {
        routingInterceptor.initialize();
        uiConfig.init();
    }

})();