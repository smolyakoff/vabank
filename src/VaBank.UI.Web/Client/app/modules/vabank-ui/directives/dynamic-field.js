(function () {
    'use strict';

    angular.module('vabank.ui').directive('dynamicField', dynamicFieldDirective);

    dynamicFieldDirective.$inject = [];

    function dynamicFieldDirective() {

        return {
            restrict: 'E',
            require: '^formFor',
            templateUrl: '/Client/app/modules/vabank-ui/templates/forms/dynamic-field.html',
            scope: {
                attribute: '@',
                template: '@?',
                options: '=?',
            },
            link: function ($scope, $element, $attributes) {
                if (!angular.isDefined($scope.attribute)) {
                    throw new Error('Attribute is not specified.');
                }
            }
        };
    }

})()