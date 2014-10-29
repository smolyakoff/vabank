(function() {
    'use strict';

    angular.module('vabank.ui').directive('vbCurrency', vbCurrency);

    vbCurrency.$inject = ['$locale'];
    
    function vbCurrency($locale) {

        var ALLOWED_INPUT_REGEX = /^[0-9., -]+$/;
        
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
        
        function link($scope, $element, $attributes, ngModelCtrl) {
            
            var validators = {
                max: function (modelValue) {
                    var max = $scope.max ? accounting.unformat($scope.max) : null;
                    return !_.isNumber(max) || modelValue <= max;
                },
                min: function (modelValue) {
                    var min = $scope.min ? accounting.unformat($scope.min) : null;
                    return !_.isNumber(min) || modelValue >= min;
                }
            };

            var format = function (value) {
                _.each(validators, function (v, k) {
                    ngModelCtrl.$setValidity(k, v(value));
                });
                return accounting.formatMoney(value, '', $scope.precision, $scope.thousand, $scope.decimal);
            };

            var parse = function (viewValue) {
                var modelValue = accounting.unformat(viewValue);
                _.each(validators, function(v, k) {
                    ngModelCtrl.$setValidity(k, v(modelValue));
                });
                return modelValue;
            };

            var $input = $element.find('input');

            ngModelCtrl.$render = function () {
                $input.val(ngModelCtrl.$viewValue);
            };
            

            $input.on('keypress', function(e) {
                var code = String.fromCharCode(e.keyCode);
                if (!ALLOWED_INPUT_REGEX.test(code)) {
                    e.preventDefault();
                }
            });

            $input.on('blur', function (e) {
                $scope.$apply(function() {
                    ngModelCtrl.$setViewValue(format(e.target.value), 'blur');
                });             
                ngModelCtrl.$render();
            });

            ngModelCtrl.$formatters.push(format);
            ngModelCtrl.$parsers.push(parse);
        }

        return {
            restrict: 'EA',
            require: 'ngModel',
            scope: {
                symbol: '@?',
                precision: '@?',
                thousand: '@?',
                decimal: '@?',
                min: '@?',
                max: '@?'
            },
            compile: compile,
            template: '<div class="input-group vb-currency">' +
                '<span class="input-group-addon">{{symbol}}</span>' +
                '<input type="text" class="form-control">' +
                '</div>'
        };
    }

})();