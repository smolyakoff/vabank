(function () {
    'use strict';

    angular.module('vabank.ui').directive('vbDateInput', vbDateInput);

    vbDateInput.$inject = ['$timeout'];

    function vbDateInput($timeout) {

        function link($scope, $element, $attributes, ngModelCtrl) {

            var formats = {
                displayFormat: $attributes.displayFormat || $attributes.inputFormat,
                inputFormat: $attributes.inputFormat
            };

            var validators = {
                moment: function(value) {
                    return value.isValid();
                }
            };
            
            var $input = $element;

            $input.unbind('input').unbind('keydown').unbind('change');

            var format = function (value) {
                var viewValue = moment(value).format(formats.displayFormat);
                return viewValue;
            };

            var parse = function (viewValue) {
                var modelValue = moment(viewValue, formats.inputFormat, true);
                if (!modelValue.isValid() || modelValue.year() < 2000) {
                    modelValue = moment(viewValue, formats.displayFormat, true);
                }
                _.each(validators, function (v, k) {
                    ngModelCtrl.$setValidity(k, v(modelValue));
                });
                if (modelValue.isValid() && modelValue.year() > 2000) {
                    return modelValue.toDate();
                } else {
                    return ngModelCtrl.$modelValue;
                }
            };
            

            ngModelCtrl.$render = function () {
                $input.val(ngModelCtrl.$viewValue);
            };

            $input.on('blur', function (e) {
                $timeout(function() {
                    ngModelCtrl.$setViewValue($input.val());
                    ngModelCtrl.$viewValue = format(ngModelCtrl.$modelValue);
                    ngModelCtrl.$render();
                }, 100);
                
            });

            $input.on('focus', function (e) {
                ngModelCtrl.$viewValue = moment(ngModelCtrl.$modelValue).format(formats.inputFormat);
                ngModelCtrl.$render();
            });

            ngModelCtrl.$formatters.unshift(format);
            ngModelCtrl.$parsers.push(parse);
        }

        return {
            restrict: 'A',
            require: 'ngModel',
            link: link,
            priority: 1
        };
    }

})();