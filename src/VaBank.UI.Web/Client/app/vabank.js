(function () {
    'use strict';
    var webapp = angular.module('vabank.webapp', [
        'ngResource',
        'ui.router',
        'ui.bootstrap'
    ]);

    webapp.run(['$state', main]);
    
    function main($state) {
        console.log('Application is running');
    }

})();