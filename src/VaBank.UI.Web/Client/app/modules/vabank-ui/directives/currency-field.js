(function() {
    'use strict';

    angular.module('vabank.ui').directive('currencyField', dateFieldDirective);

    dateFieldDirective.$inject = ['FieldHelper', '$locale'];

    function dateFieldDirective(FieldHelper, $locale) {
        
        function compile(element, attributes) {
            var defaults = {
                symbol: $locale.NUMBER_FORMATS.CURRENCY_SYM,
                precision: $locale.NUMBER_FORMATS.PATTERNS[1].maxFrac,
                thousand: $locale.NUMBER_FORMATS.GROUP_SEP,
                decimal: $locale.NUMBER_FORMATS.DECIMAL_SEP,
            };
            angular.extend(defaults, attributes);
            angular.extend(attributes, defaults);


            return link;
        }
        
        function link($scope, $element, $attributes, formForController) {
            FieldHelper.manageLabel($scope, $attributes);
            FieldHelper.manageFieldRegistration($scope, $attributes, formForController);
        }

        return {
            restrict: 'E',
            require: '^formFor',
            templateUrl: '/Client/app/modules/vabank-ui/templates/forms/currency-field.html',
            scope: {
                attribute: '@',
                disable: '=',
                debounce: '@?',
                help: '@?',
                placeholder: '@?',
                symbol: '@?',
                precision: '@?',
                thousand: '@?',
                decimal: '@?',
                min: '@?',
                max: '@?'
            },
            compile: compile 
        };
    }

})()