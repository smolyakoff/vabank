(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('profileEditController', profileEditController);

    profileEditController.$inject = ['$scope', 'profileService', 'uiTools', 'data']; 

    function profileEditController($scope, profileService, uiTools, data) {
        $scope.profileForm = data;

        $scope.validationRules = {
            userName: { required: true, custom: uiTools.validate.getValidator('userName') },
            password: { custom: uiTools.validate.getValidator('password') },
            passwordConfirmation: {
                custom: uiTools.validate.getValidator('passwordConfirmation',  {passwordField: 'password'})
            },
            email: { required: true, type: 'email' },
            firstName: { required: true },
            lastName: { required: true },
            phoneNumber: { custom: uiTools.validate.getValidator('phone') },
        };

        $scope.save = function() {
            var promise = profileService.Profile.save($scope.profileForm).$promise;
            return uiTools.validate.handleServerResponse(promise);
        };

        $scope.onSaved = function() {
            uiTools.notify({
                type: 'success',
                message: 'Информация профиля обновлена'
            });
        };
    }
})();
