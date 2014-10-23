(function() {
    'use strict';

    angular
        .module('vabank.ui')
        .directive('vbDatepicker', vbDatepicker);

    vbDatepicker.$inject = [];
    
    function vbDatepicker() {

        controller.$inject = ['$scope'];
        function controller($scope) {
            $scope.dateFormat = $scope.dateFormat || 'LLLL';
            $scope.iconSide = $scope.iconSide || 'right';
        };

        function link($scope, $element, $attrs) {
        };

        var directive = {
            link: link,
            controller: controller,
            restrict: 'EA',
            require: '^ngModel',
            templateUrl: '/Client/app/modules/vabank-ui/templates/vb-datepicker.html',
            scope: {
                iconSide: '@?',
                ngModel: '=',
                config: '=',
                dateFormat: '@?',
                placeholder: '@'
            }
        };
        return directive;
    }

})();