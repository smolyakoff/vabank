(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('loginController', loginController);

    loginController.$inject = ['$scope', '$q', 'uiTools', 'authService']; 

    function loginController($scope, $q, uiTools, authService) {

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
                debugger;
            }
            
            function onError() {
                debugger;
            }

            authService.login($scope.loginForm)
                .then(onSuccess, onError);

        };

    }
})();
