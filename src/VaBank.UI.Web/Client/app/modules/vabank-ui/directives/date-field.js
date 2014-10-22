(function() {
    'use strict';

    angular.module('vabank.ui').directive('dateField', dateFieldDirective);

    dateFieldDirective.$inject = ['FieldHelper'];

    function dateFieldDirective(FieldHelper) {
        return {
            restrict: 'E',
            require: '^formFor',
            templateUrl: '/Client/app/modules/vabank-ui/templates/forms/date-field.html',
            scope: {
                attribute: '@',
                disable: '=',
                dateConfig: '=',
                dateFormat: '@?',
                debounce: '@?',
                help: '@?',
                placeholder: '@?'
            },
            link: function ($scope, $element, $attributes, formForController) {
                FieldHelper.manageLabel($scope, $attributes);
                FieldHelper.manageFieldRegistration($scope, $attributes, formForController);
            }
        };
    }

})()