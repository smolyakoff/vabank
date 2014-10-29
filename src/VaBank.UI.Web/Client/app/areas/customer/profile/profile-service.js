(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('profileService', profileService);

    profileService.$inject = ['$resource', 'authService'];

    function profileService($resource, authService) {

        var getUserId = function() {
            return authService.getUser().id;
        };

        var Profile = $resource('/api/users/:userId/profile', { userId: getUserId }, {            
            save: {
                method: 'PUT'
            },
            changePassword: {
                url: '/api/users/:userId/profile/change-password',
                method: 'POST'
            }
        });

        return {            
            Profile: Profile
        };
    }
})();