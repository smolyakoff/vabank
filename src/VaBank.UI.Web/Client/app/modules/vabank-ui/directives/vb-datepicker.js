(function() {
    'use strict';

    angular
        .module('vabank.ui')
        .directive('vbDatepicker', vbDatepicker);

    vbDatepicker.$inject = [];
    
    function vbDatepicker() {

        function postLink($scope, $element, $attrs) {
        };
        
        function compile(element, attributes) {
            attributes.iconPlacement = attributes.iconPlacement || 'right';
            attributes.dateFormat = attributes.dateFormat || 'LLLL';
        }

        var directive = {
            compile: compile,
            restrict: 'EA',
            require: '^ngModel',
            templateUrl: '/Client/app/modules/vabank-ui/templates/vb-datepicker.html',
            scope: {
                iconPlacement: '@?',
                ngModel: '=',
                config: '=',
                dateFormat: '@?',
                dateDisplayFormat: '@?',
                placeholder: '@?'
            }
        };
        return directive;
    }

})();