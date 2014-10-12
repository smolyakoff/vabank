(function () {
    'use strict';

    angular.module('vabank.ui').factory('formForTemplateConfig', formForTemplateConfig);

    formForTemplateConfig.$inject = ['$templateCache'];

    function formForTemplateConfig($templateCache) {

        var textFieldHtml = document.getElementById('text-field-template').text;
        var checkboxHtml = document.getElementById('checkbox-field-template').text;

        var init = function () {
            $templateCache.put('form-for/templates/text-field.html', textFieldHtml);
            $templateCache.put('form-for/templates/checkbox-field.html', checkboxHtml);
        };

        return {
            init: init
        };
    }
})()