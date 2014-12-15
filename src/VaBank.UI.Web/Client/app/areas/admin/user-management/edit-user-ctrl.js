(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('editUserController', editUserController);

    editUserController.$inject = ['$scope', '$q', '$state', '$stateParams', 'userManagementService', 'uiTools', 'data']; 

    function editUserController($scope, $q, $state, $stateParams, userManager, uiTools, data) {

        var createForm = function(data) {
            var form = angular.extend({}, data.user, data.profile, data.paymentProfile);
            form.role = form.role || _.find(data.user.claims, function(x) {
                return x.type.indexOf('role') > 0;
            }).value;
            return form;
        };

        $scope.isEdit = $stateParams.id !== 'add';
        $scope.lookup = {
            roles: _.map(userManager.User.defaults.roles, function (x) {
                return { value: x, label: x };
            })
        };
        $scope.userForm = createForm(data);

        $scope.save = function () {
            var promise = $scope.isEdit
                ? userManager.User.save($scope.userForm).$promise
                : userManager.User.create($scope.userForm).$promise;
            return uiTools.validate.handleServerResponse(promise);
        };

        $scope.onSaved = function(response) {
            var message = $scope.isEdit
                ? uiTools.format('Пользователь {0} был успешно сохранен', response.userName)
                : uiTools.format('Пользователь {0} был успешно создан', response.userName);
            uiTools.notify({
                type: 'success',
                message: message,
            });
            $state.go('admin.userManagement.list');
        };

        var isChangePasswordMode = function(value, model) {
            return !$scope.isEdit || model.changePassword;
        };

        $scope.validationRules = {
            userName: {required: true, custom: uiTools.validate.getValidator('userName') },
            password: { custom: uiTools.validate.getConditionalValidator('password', isChangePasswordMode) },
            role: { required: true },
            passwordConfirmation: {
                custom: uiTools.validate.getConditionalValidator('passwordConfirmation', isChangePasswordMode, {
                    passwordField: 'password'
                })
            },
            email: { required: true, type: 'email' },
            firstName: { required: true },
            lastName: { required: true},
            phoneNumber: { custom: uiTools.validate.getOptionalValidator('phone') },
            secretPhrase: { required: true },
            fullName: { required: true },
            address: {required: true}
        };
    }
})();
