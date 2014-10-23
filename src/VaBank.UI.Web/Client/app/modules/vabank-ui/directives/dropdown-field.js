(function() {
    'use strict';

    angular.module('vabank.ui').directive('dropdownField', dropdownFieldDirective);

    dropdownFieldDirective.$inject = ['FieldHelper'];
    function dropdownFieldDirective(FieldHelper) {

        function link($scope, $element, $attributes, formForController, transcludeFn) {
            if (!$scope.repeat) {
                throw new Error('Repeat expression for dropdown field should be specified');
            }
            FieldHelper.manageLabel($scope, $attributes);
            FieldHelper.manageFieldRegistration($scope, $attributes, formForController);
            //Hack: do not really know how to do a correct transclusion here
            if ($attributes.exportScope) {
                var exported = $attributes.exportScope.split(',');
                _.forEach(exported, function (x) {
                    $scope[x] = $scope.$parent[x];
                });
            }
        }

        controller.inject = ['$scope'];
        function controller($scope) {
            $scope.refresh = $scope.refresh || angular.noop;
        }

        return {
            restrict: 'E',
            require: '^formFor',
            transclude: true,
            templateUrl: '/Client/app/modules/vabank-ui/templates/forms/dropdown-field.html',
            scope: {
                items: '=',
                refresh: '=?',
                repeat: '@',
                placeholder: '@',
                attribute: '@',
                displayAttribute: '@?',
                disable: '=',
                help: '@?',
            },
            link: link,
            controller: controller
        };
    }

    


})();