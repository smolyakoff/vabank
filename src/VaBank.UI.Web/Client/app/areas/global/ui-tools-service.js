(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('uiTools', uiTools);

    uiTools.$inject = ['$filter', 'controlUtil', 'dataUtil', 'validationService', 'notificationService', 'promiseTracker'];

    function uiTools($filter, controlUtil, dataUtil, validationService, notificationService, promiseTracker) {
        return {
            control: controlUtil,
            manipulate: dataUtil,
            validate: validationService,
            notify: notificationService.notify,
            promiseTracker: promiseTracker,
            format: $filter('stringFormat')
        };
    }
})();