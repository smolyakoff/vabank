(function() {
    'use strict';

    angular
        .module('vabank.ui')
        .directive('vbDatepicker', vbDatepicker);

    vbDatepicker.$inject = [];
    
    function vbDatepicker() {

        var link = function (scope, element, attrs) {
        };

        var directive = {
            link: link,
            restrict: 'EA',
            require: '^ngModel',
            templateUrl: '/Client/app/modules/vabank-ui/templates/vb-datepicker.html',
            scope: {
                ngModel: '=',
                config: '=',
                dateFormat: '@'
            }
        };
        return directive;
    }

})();