(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('loginController', loginController);

    loginController.$inject = ['$scope', '$q', 'uiTools']; 

    function loginController($scope, $q, uiTools) {

        $scope.loginForm = {
            login: null,
            password: null
        };

        $scope.validationRules = {
            login: {
                required: true,
                custom: uiTools.validate.getValidator('login')
            },
            password: {
                required: true,
                custom: uiTools.validate.getValidator('password')
            }
        };

        $scope.validationFailed = function() {

        };

        $scope.login = function () {
            uiTools.notify({
                type: 'error',
                message: 'Not implemented'
            });
        };

    }
})();
