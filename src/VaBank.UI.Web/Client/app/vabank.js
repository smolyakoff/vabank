(function () {
    'use strict';
    var webapp = angular.module('vabank.webapp', [
        'ngResource',
        'ui.router',
        'LocalStorageModule',
        'ui.bootstrap',
        'ui.bootstrap.datetimepicker',
        'ui.dateTimeInput',
        'cfp.loadingBar',
        'ajoslin.promise-tracker',
        'angularSpinner',
        'multi-select',
        'smart-table',
        'formFor', 
        'formFor.bootstrapTemplates',
        
        'vabank.ui'
    ]);

    webapp.run(['routingInterceptor', 'formForConfig', main]);
    
    function main(routingInterceptor, formConfig) {
        routingInterceptor.initialize();
        formConfig.init();
    }

})();