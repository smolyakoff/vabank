(function() {
    'use strict';

    angular.module('vabank.ui').directive('dropdownField', dropdownFilterDirective);

    dropdownFilterDirective.$inject = ['FieldHelper'];

    function dropdownFilterDirective(FieldHelper) {

        link.$inject = ['$scope', '$element', '$attributes', 'formForController'];
        function link($scope, $element, $attributes, formForController, transcludeFn) {
            FieldHelper.manageLabel($scope, $attributes);
            FieldHelper.manageFieldRegistration($scope, $attributes, formForController);

            transcludeFn(function (clone) {
                clone.wrap('<ui-select ng-model="model.bindable"></ui-select>');
                $element.after(clone);
            });
            debugger;
        }

        return {
            restrict: 'E',
            require: '^formFor',
            transclude: true,
            templateUrl: '/Client/app/modules/vabank-ui/templates/forms/dropdown-field.html',
            scope: {
                attribute: '@',
                disable: '=',
                help: '@?',
            },
            link: link
        };
    }

})()