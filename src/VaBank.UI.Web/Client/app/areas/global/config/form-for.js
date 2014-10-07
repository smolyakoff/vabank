(function() {
    'use strict';

    angular.module('vabank.webapp').factory('formForConfig', formForConfig);

    formForConfig.$inject = ['FormForConfiguration'];

    function formForConfig(config) {
        var init = function() {
            config.setValidationFailedForRequiredMessage('Обязательное поле');
            //config.setDefaultDebounceDuration(false);
        };

        return {
            init: init
        };
    }
})()