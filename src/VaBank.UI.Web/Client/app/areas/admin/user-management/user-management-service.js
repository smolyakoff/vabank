(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('userManagementService', userManagementService);

    userManagementService.$inject = ['$resource', 'dataUtil', 'uiTools'];

    function userManagementService($resource, dataUtil, uiTools) {

        var User = $resource('/api/users/:userId', { userId: '@userId' }, {
            save: {
              method: 'PUT'  
            },
            query: {
                isArray: false,
                params: {
                    pageNumber: 1,
                    pageSize: 15,
                    sort: 'userName asc'
                }
            },
            create: {
                url: '/api/users',
                method: 'POST',
            },
        });

        User.defaults = {};
        User.defaults.filter = function() {
            return {
                userName: {
                    propertyName: 'userName',
                    propertyType: 'string',
                    operator: dataUtil.filters.operator.Contains,
                    value: ''
                },
                email: {
                    propertyName: 'email',
                    propertyType: 'string',
                    operator: dataUtil.filters.operator.Contains,
                    value: ''
                },
                firstName: {
                    propertyName: 'firstName',
                    propertyType: 'string',
                    operator: dataUtil.filters.operator.Contains,
                    value: ''
                },
                lastName: {
                    propertyName: 'lastName',
                    propertyType: 'string',
                    operator: dataUtil.filters.operator.Contains,
                    value: ''
                }
            };
        };
        User.defaults.sort = 'userName asc';
        User.defaults.new = { role: 'Customer' };
        User.defaults.roles = ['Admin', 'Customer'];

        User.search = function (options) {
            var params = {};
            var paged = false;
            if (options.pageNumber && options.pageSize) {
                params.pageNumber = options.pageNumber;
                params.pageSize = options.pageSize;
                paged = true;
            } else {
                params.pageSize = 1;
                params.pageSize = 10000000;
            }
            if (options.searchString) {
                var filters = angular.copy(User.defaults.filter());
                _.forEach(filters, function(x) {
                    x.value = options.searchString;
                });
                var linq = dataUtil.filters.combine(filters, dataUtil.filters.logic.Or)
                    .toLINQ();
                params.filter = linq;
            }
            params.sort = options.sort || User.defaults.sort;
            params.roles = options.roles;
            if (paged) {
                return User.query(params).$promise;
            } else {
                return User.query(params).$promise.then(function(page) {
                    return page.items;
                });
            }
        };
        User.format = function(user) {
            var userString = uiTools.format("{0} {1} ({2})", user.firstName, user.lastName, user.email);
            return userString;
        };

        var Profile = $resource('/api/users/:userId/profile', { userId: '@userId' });
        Profile.defaults = {};
        Profile.defaults.new = {};

        return {            
            User: User,
            Profile: Profile
        };
    }
})();