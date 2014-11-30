(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .factory('uiTools', uiTools);

    uiTools.$inject = ['$filter', 'controlUtil', 'dataUtil', 'validationService', 'notificationService', 'promiseTracker', 'dialogService'];

    function uiTools($filter, controlUtil, dataUtil, validationService, notificationService, promiseTracker, dialogService) {
        return {
            control: controlUtil,
            manipulate: dataUtil,
            validate: validationService,
            notify: notificationService.notify,
            dialog: dialogService,
            promiseTracker: promiseTracker,
            format: $filter('stringFormat')
        };
    }
})();