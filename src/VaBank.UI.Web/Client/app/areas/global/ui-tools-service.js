(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('uiTools', uiTools);

    uiTools.$inject = ['controlUtil', 'dataUtil', 'validationService', 'notificationService', 'promiseTracker'];

    function uiTools(controlUtil, dataUtil, validationService, notificationService, promiseTracker) {
        return {
            control: controlUtil,
            manipulate: dataUtil,
            validate: validationService,
            notify: notificationService.notify,
            promiseTracker: promiseTracker
        };
    }
})();