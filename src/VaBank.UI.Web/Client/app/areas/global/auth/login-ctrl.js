(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('loginController', loginController);

    loginController.$inject = ['$scope', 'uiTools']; 

    function loginController($scope, uiTools) {

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

        $scope.login = function() {
            uiTools.notify({
                type: 'error',
                message: 'Not implemented'
            });
        };

    }
})();
