(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('changePasswordController', changePasswordController);

    changePasswordController.$inject = ['$scope', 'uiTools', 'authService', 'profileService']; 

    function changePasswordController($scope, uiTools, authService, profileService) {
        var user = authService.getUser();

        $scope.formController = {};

        $scope.changePwdForm = {
            userId: user.id
        };

        $scope.validationRules = {
            currentPassword: { required: true },
            newPassword: {
                custom: uiTools.validate.combineValidators([
                    uiTools.validate.createValidator(function(value, model) {
                        if (value === model.currentPassword) {
                            return 'Новый и старый пароли должны различаться';
                        }
                        return null;
                    }),
                    uiTools.validate.getValidator('password')
                ])
            },
            newPasswordConfirmation: {
                custom: uiTools.validate.getValidator('passwordConfirmation', {
                    passwordField: 'newPassword'
                })
            },
        };

        $scope.change = function() {
            var promise = profileService.Profile.changePassword($scope.changePwdForm).$promise;
            return uiTools.validate.handleServerResponse(promise);
        };

        $scope.onError = function (error) {
            $scope.formController.resetFields();
            if (error.status === 403 && error.data.errorType === 'security') {
                uiTools.notify({
                    type: 'error',
                    message: "Cтарый пароль введен неверно."
                });
            }
        };

        $scope.onChanged = function (data) {
            $scope.formController.resetFields();
            uiTools.notify({                
                title: 'Пароль изменен',
                message: data.message
            });
        };

    }
})();
