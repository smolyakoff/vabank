(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('cardManagementService', cardManagementService);

    cardManagementService.$inject = ['$resource', 'dataUtil', 'uiTools', 'userManagementService'];

    function cardManagementService($resource, dataUtil, uiTools, userManagementService) {

        var CardAccount = $resource('/api/accounts/card/:accountNo', { accountNo: '@accountNo' }, {
            query: {
                isArray: false,
                params: {sort: 'openDateUtc DESC'}
            },
            lookup: {
                url: '/api/accounts/card/lookup'
            }
            
        });
        CardAccount.defaults = {};
        CardAccount.defaults.filter = function() {
            return {                
                accountNo: {
                    propertyName: 'accountNo',
                    type: 'string',
                    operator: dataUtil.filters.operator.Contains,
                    value: ''
                },
                userName: {
                    propertyName: 'owner.userName',
                    type: 'string',
                    operator: dataUtil.filters.operator.Contains,
                    value: ''
                },
                firstName: {
                    propertyName: 'owner.firstName',
                    type: 'string',
                    operator: dataUtil.filters.operator.Contains,
                    value: ''
                },
                lastName: {
                    propertyName: 'owner.lastName',
                    type: 'string',
                    operator: dataUtil.filters.operator.Contains,
                    value: ''
                },
                email: {
                    propertyName: 'owner.email',
                    type: 'string',
                    operator: dataUtil.filters.operator.Contains,
                    value: ''
                }
            };
        };
        CardAccount.search = function(options) {
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
                var filters = angular.copy(CardAccount.defaults.filter());
                _.forEach(filters, function (x) {
                    x.value = options.searchString;
                });
                var linq = dataUtil.filters
                    .combine(filters, dataUtil.filters.logic.Or)
                    .toLINQ();
                params.filter = linq;
            }
            params.sort = options.sort || CardAccount.defaults.sort;
            params.roles = options.roles;
            if (paged) {
                return CardAccount.query(params).$promise;
            } else {
                return CardAccount.query(params).$promise.then(function (page) {
                    return page.items;
                });
            }
        };

        var AccountCard = $resource('/api/accounts/card/:accountNo/cards', { accountNo: '@accountNo' }, {
            create: {
                url: '/api/accounts/card/:accountNo/cards',
                method: 'POST',
            },
            assign: {
                url: '/api/accounts/card/:accountNo/assign',
                method: 'POST'
            }
        });

        var Card = $resource('/api/cards/:cardId', { cardId: '@cardId' }, {
            queryUnassigned: {
                params: {
                    filter: dataUtil.filters.create({
                        propertyName: 'accountNo',
                        operator: dataUtil.filters.operator.Equal,
                        value: null
                    }).toLINQ()
                },
                isArray: true
            }
        });


        return {
            AccountCard: AccountCard,
            CardAccount: CardAccount,
            Card: Card,
            User: userManagementService.User
        };
    }
})();