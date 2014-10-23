(function () {
    'use strict';

    angular.module('vabank.webapp').controller('homeController', homeController);

    homeController.$inject = ['$state','authService'];

    function homeController($state, authService) {

        var user = authService.getUser();
        if (!user.isAuthenticated()) {
            $state.go('login');
        } else if (user.isInRole('Customer')) {
            $state.go('customer.cabinet');
        } else if (user.isInRole('Admin')) {
            $state.go('admin');
        } else {
            $state.go('login');
        }

    }

})();