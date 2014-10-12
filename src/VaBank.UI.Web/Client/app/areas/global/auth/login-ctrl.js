(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('loginController', loginController);

    loginController.$inject = ['$scope', '$q', '$state', '$stateParams', 'uiTools', 'authService']; 

    function loginController($scope, $q, $state, $stateParams, uiTools, authService) {


        $scope.loginForm = {
            login: null,
            password: null
        };

        $scope.validationRules = {
            login: { required: true },
            password: { required: true }
        };


        $scope.login = function () {
           
            function onSuccess() {
                var user = authService.getUser();
                if ($stateParams.redirect) {
                    $state.go($stateParams.redirect, $stateParams.redirectParams);
                } else if (user.isInRole('Admin')) {
                    $state.go('admin');
                } else {
                    $state.go('customer.cabinet');
                }
            }
            
            function onError(response) {
                var message = '';
                var error = JSON.parse(response.error_description);
                if (response.error === 'LoginValidationError') {
                    message = _.pluck(error.faults, 'message').join('\r\n');
                } else {
                    message = error.message;
                }
                uiTools.notify({
                    type: 'error',
                    title: 'Не удалось войти',
                    message: message
                });
                $scope.loginForm.password = null;
            }
            
            return authService
                .login($scope.loginForm)
                .then(onSuccess, onError);

        };
    }
})();
