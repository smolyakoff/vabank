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
            var user = _.findWhere($scope.users, { isSelected: true });
            return user;
        };

        var filters = angular.copy(userManager.User.defaults.filter);

        var lastParams = [];
        
        $scope.lookup = {
            roles: uiTools.control.multiselect.getSelectChoices(['Admin', 'Customer'])
        };

        $scope.loading = uiTools.promiseTracker();

        $scope.search = '';

        $scope.users = [];

        $scope.pageSize = 15;

        $scope.getRole = function(user) {
            return _.find(user.claims, function(x) {
                return x.type.indexOf('role') > 0;
            }).value;
        };

        $scope.isUserSelected = function() {
            return !_.isUndefined(getSelectedUser());
        };

        $scope.query = function (tableState) {
            var params = angular.extend(
                {},
                queryService.fromStTable(tableState),
                lastParams);
            var promise =  userManager.User.query(params).$promise;
            $scope.loading.addPromise(promise);
            promise.then(function (page) {
                tableState.pagination.start = page.skip;
                tableState.pagination.numberOfPages = page.totalPages;
                $scope.users = page.items;
            });
        };

        $scope.show = function() {
            var params = {
                roles: multiselectService.getSelectedItems($scope.lookup.roles),
                pageNumber: 1,
            };
            if ($scope.search) {
                _.forEach(filters, function(x) {
                    x.value = $scope.search;
                });
                params.filter = filterService.combine(filters, filterService.logic.Or).toLINQ();
            }
            lastParams = params;
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
