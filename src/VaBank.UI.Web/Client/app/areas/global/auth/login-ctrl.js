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
            login: {
                custom: uiTools.validate.getValidator('login')
            },
            password: {
                required: true,
                //custom: uiTools.validate.getValidator('password')
            }
        };

        $scope.validationFailed = function() {

        };

        $scope.login = function () {
            function onSuccess() {
                var user = authService.getUser();
                if (user.isInRole('Admin')) {
                    $state.go('admin');
                } else {
                    $state.go('customer.cabinet');
                }
            }
            
            authService.login($scope.loginForm)
                .then(onSuccess);

        };
    }
})();
