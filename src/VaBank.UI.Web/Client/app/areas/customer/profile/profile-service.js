(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('profileService', profileService);

    profileService.$inject = ['$resource'];

    function profileService($resource) {
        var Profile = $resource('/api/users/:userId/profile', { userId: '@userId' }, {            
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