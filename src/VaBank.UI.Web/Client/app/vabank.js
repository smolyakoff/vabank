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

    webapp.run(['routingInterceptor', 'notificationService', 'serverInfo', main]);
    
    function main(routingInterceptor, notifier, serverInfo) {
        if (serverInfo.isDebug) {
            notifier.notify({
                state: 'cabinet',
                type: 'info',
                message: 'Приложение загружено.'
            });
        }
        routingInterceptor.initialize();
    }

})();