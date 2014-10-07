(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('uiTools', uiTools);

    uiTools.$inject = ['controlUtil', 'dataUtil', 'validationService', 'notificationService'];

    function uiTools(controlUtil, dataUtil, validationService, notificationService) {
        return {
            control: controlUtil,
            manipulate: dataUtil,
            validate: validationService,
            notify: notificationService.notify
        };
    }
})();