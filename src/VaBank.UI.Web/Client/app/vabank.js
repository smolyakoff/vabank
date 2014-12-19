(function () {
    'use strict';
    var webapp = angular.module('vabank.webapp', [
        'ngResource',
        'ngSanitize',
        'ngAnimate',
        'ui.router',
        'ui.mask',
        'LocalStorageModule',
        'ui.bootstrap',
        'ui.bootstrap.datetimepicker',
        'ui.dateTimeInput',
        'ui.select',
        'cfp.loadingBar',
        'ajoslin.promise-tracker',
        'angularSpinner',
        'angularMoment',
        'multi-select',
        'smart-table',
        'formFor', 
        'formFor.bootstrapTemplates',
        'mgo-angular-wizard',
        'matchMedia',
        'googlechart',
        
        'vabank.ui',
        'vabank.auth'
    ]);

    webapp.run(['routingInterceptor', 'uiConfig', main]);
    
    function main(routingInterceptor, uiConfig) {
        routingInterceptor.initialize();
        uiConfig.init();
    }

})();