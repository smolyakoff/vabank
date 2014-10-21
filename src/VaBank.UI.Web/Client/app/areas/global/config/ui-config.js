(function() {
    'use strict';

    angular.module('vabank.webapp').factory('uiConfig', uiConfig);

    uiConfig.$inject = ['FormForConfiguration', 'uiSelectConfig'];

    function uiConfig(formForConfig, uiSelectConfig) {
        var init = function() {
            formForConfig.setValidationFailedForRequiredMessage('Обязательное поле');
            formForConfig.setValidationFailedForEmailTypeMessage('Некорректный email-адрес');
            formForConfig.setDefaultDebounceDuration(false);
            uiSelectConfig.theme = 'bootstrap';
        };

        return {
            init: init
        };
    }
})()