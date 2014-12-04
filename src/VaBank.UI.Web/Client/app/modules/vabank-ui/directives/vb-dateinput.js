(function () {
    'use strict';

    angular.module('vabank.ui').directive('vbDateInput', vbDateInput);

    vbDateInput.$inject = [];

    function vbDateInput() {

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

            var format = function (value, focused) {
                var modelValue;
                if (_.isString(value)) {
                    modelValue = moment(value, formats.displayFormat);
                    if (!modelValue.isValid()) {
                        modelValue = moment(value, formats.inputFormat);
                    }
                } else {
                    modelValue = moment(value);
                }
                _.each(validators, function (v, k) {
                    ngModelCtrl.$setValidity(k, v(modelValue));
                });
                var formatString = focused ? formats.inputFormat : formats.displayFormat;
                return modelValue.isValid() ? modelValue.format(formatString) : '';
            };

            var parse = function (viewValue) {          
                var modelValue = moment(viewValue, formats.inputFormat);
                if (!modelValue.isValid()) {
                    modelValue = moment(viewValue, formats.displayFormat);
                }
                _.each(validators, function (v, k) {
                    ngModelCtrl.$setValidity(k, v(modelValue));
                });
                return modelValue.isValid() ? modelValue : null;
            };

            

            ngModelCtrl.$render = function () {
                $input.val(ngModelCtrl.$viewValue);
            };

            $input.on('blur', function (e) {
               ngModelCtrl.$viewValue = moment(ngModelCtrl.$modelValue).format(formats.displayFormat);
               ngModelCtrl.$render();
                
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
        };
    }

})();