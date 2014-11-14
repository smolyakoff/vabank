(function() {
    'use strict';


    angular.module('vabank.ui').directive('maskedField', maskedField);

    maskedField.$inject = ['$log', '$timeout', 'FieldHelper'];

    function maskedField($log, $timeout, FieldHelper) {
        return {
            require: '^formFor',
            restrict: 'EA',
            templateUrl: '/Client/app/modules/vabank-ui/templates/forms/masked-field.html',
            scope: {
                attribute: '@',
                debounce: '@?',
                disable: '=',
                focused: '&?',
                blurred: '&?',
                mask: '@?',
                help: '@?',
                iconAfterClicked: '&?',
                iconBeforeClicked: '&?',
                placeholder: '@?'
            },
            link: function($scope, $element, $attributes, formForController) {
                if (!$scope.attribute) {
                    $log.error('Missing required field "attribute"');
                    return;
                }
                
                if (!$scope.attribute) {
                    $log.error('Missing required field "mask"');
                    return;
                }

                $scope.tabIndex = $attributes.tabIndex || 0;
                $scope.mask = $attributes.mask;

                if ($attributes.hasOwnProperty('autofocus')) {
                    $timeout(function() {
                        $element.find($scope.multiline ? 'textarea' : 'input')[0].focus();
                    });
                }

                FieldHelper.manageLabel($scope, $attributes);
                FieldHelper.manageFieldRegistration($scope, $attributes, formForController);

                // Update $scope.iconAfter based on the field state (see class-level documentation for more)
                if ($attributes.iconAfter) {
                    var updateIconAfter = function() {
                        if (!$scope.model) {
                            return;
                        }

                        var iconAfter =
                            $attributes.iconAfter.charAt(0) === '{' ?
                                $scope.$eval($attributes.iconAfter) :
                                $attributes.iconAfter;

                        if (angular.isObject(iconAfter)) {
                            if ($scope.model.error) {
                                $scope.iconAfter = iconAfter.invalid;
                            } else if ($scope.model.pristine) {
                                $scope.iconAfter = iconAfter.pristine;
                            } else {
                                $scope.iconAfter = iconAfter.valid;
                            }
                        } else {
                            $scope.iconAfter = iconAfter;
                        }
                    };

                    $attributes.$observe('iconAfter', updateIconAfter);
                    $scope.$watch('model.error', updateIconAfter);
                    $scope.$watch('model.pristine', updateIconAfter);
                }

                // Update $scope.iconBefore based on the field state (see class-level documentation for more)
                if ($attributes.iconBefore) {
                    var updateIconBefore = function() {
                        if (!$scope.model) {
                            return;
                        }

                        var iconBefore =
                            $attributes.iconBefore.charAt(0) === '{' ?
                                $scope.$eval($attributes.iconBefore) :
                                $attributes.iconBefore;

                        if (angular.isObject(iconBefore)) {
                            if ($scope.model.error) {
                                $scope.iconBefore = iconBefore.invalid;
                            } else if ($scope.model.pristine) {
                                $scope.iconBefore = iconBefore.pristine;
                            } else {
                                $scope.iconBefore = iconBefore.valid;
                            }
                        } else {
                            $scope.iconBefore = iconBefore;
                        }
                    };

                    $attributes.$observe('iconBefore', updateIconBefore);
                    $scope.$watch('model.error', updateIconBefore);
                    $scope.$watch('model.pristine', updateIconBefore);
                }

                $scope.onIconAfterClick = function() {
                    if ($attributes.hasOwnProperty('iconAfterClicked')) {
                        $scope.iconAfterClicked();
                    }
                };
                $scope.onIconBeforeClick = function() {
                    if ($attributes.hasOwnProperty('iconBeforeClicked')) {
                        $scope.iconBeforeClicked();
                    }
                };
                $scope.onFocus = function() {
                    if ($attributes.hasOwnProperty('focused')) {
                        $scope.focused();
                    }
                };
                $scope.onBlur = function() {
                    if ($attributes.hasOwnProperty('blurred')) {
                        $scope.blurred();
                    }
                };
            }
        };
    }

})();
