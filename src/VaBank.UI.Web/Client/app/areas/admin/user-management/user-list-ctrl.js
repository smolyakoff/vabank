(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('userListController', userListController);

    userListController.$inject = ['$scope', '$state', 'uiTools', 'userManagementService']; 

    function userListController($scope, $state, uiTools, userManager) {

        var filterService = uiTools.manipulate.filters;
        var queryService = uiTools.manipulate.query;
        var multiselectService = uiTools.control.multiselect;

        var getSelectedUser = function() {
            var user = _.findWhere($scope.displayedUsers, { isSelected: true });
            return user;
        };

        var filters = angular.copy(userManager.User.defaults.filter);

        var lastParams = [];
        
        $scope.lookup = {
            roles: uiTools.control.multiselect.getSelectChoices(['Admin', 'Customer'])
        };

        $scope.loading = uiTools.promiseTracker();

        $scope.search = '';

        $scope.users = [
            { id: '1', userName: 'smolyakoff1', firstName: 'Константин', lastName: 'Смоляков', roles: ['Admin'], email: 'smolyakoff@tut.by' },
            { id: '1', userName: 'smolyakoff2', firstName: 'Константин', lastName: 'Смоляков', roles: ['Admin'], email: 'smolyakoff@tut.by' },
            { id: '1', userName: 'smolyakoff3', firstName: 'Константин', lastName: 'Смоляков', roles: ['Admin'], email: 'smolyakoff@tut.by' },
            { id: '1', userName: 'smolyakoff4', firstName: 'Константин', lastName: 'Смоляков', roles: ['Admin'], email: 'smolyakoff@tut.by' },
            { id: '1', userName: 'smolyakoff5', firstName: 'Константин', lastName: 'Смоляков', roles: ['Admin'], email: 'smolyakoff@tut.by' },
            { id: '1', userName: 'smolyakoff6', firstName: 'Константин', lastName: 'Смоляков', roles: ['Admin'], email: 'smolyakoff@tut.by' }
        ];

        $scope.displayedUsers = angular.copy($scope.users);

        $scope.isUserSelected = function() {
            return !_.isUndefined(getSelectedUser());
        };

        $scope.query = function (tableState) {
            var params = angular.extend(
                {},
                lastParams,
                queryService.fromStTable(tableState));
            debugger;
            tableState.pagination.numberOfPages = 3;
        };

        $scope.show = function() {
            var params = {
                roles: multiselectService.getSelectedItems($scope.lookup.roles)
            };
            if ($scope.search) {
                _.forEach(filters, function(x) {
                    x.value = $scope.search;
                });
                params.filter = filterService.combine(filters, filterService.logic.Or).toLINQ();
            }
            lastParams = params;
            debugger;
        };

        $scope.add = function () {
            $state.go('admin.userManagement.editUser', {id: 'add'});
        };

        $scope.edit = function() {
            var selectedUser = getSelectedUser();
            $state.go('admin.userManagement.editUser', { id: selectedUser.id });
        };
    }
})();
