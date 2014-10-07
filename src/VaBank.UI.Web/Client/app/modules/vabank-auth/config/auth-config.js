(function() {
    'use strict';

    angular.module('vabank.auth').constant('authConfig', {        
        clientId: 'vabank.website',
        storageKey: 'vabank.auth',
        apiUrl: '/api/token'
    });
})