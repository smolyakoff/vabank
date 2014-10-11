(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('userManagementService', userManagementService);

    userManagementService.$inject = ['$resource', 'dataUtil'];

    function userManagementService($resource, dataUtil) {

        var User = $resource('/api/users', {}, {            
            query: {
                isArray: false,
                params: {
                    pageNumber: 1,
                    pageSize: 15,
                    sort: 'userName asc'
                }
            }
        });
        User.defaults = {};
        User.defaults.filter = {            
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
        User.defaults.new = { profile: {} };
        User.defaults.roles = ['Admin', 'Customer'];

        return {            
            User: User,
        };
    }
})();