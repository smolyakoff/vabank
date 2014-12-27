(function() {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('userListController', userListController);

    userListController.$inject = ['$scope', '$state', '$q', 'uiTools', 'userManagementService', '$modal'];

    function userListController($scope, $state, $q, uiTools, userManager, $modal) {

        var User = userManager.User;
        var Profile = userManager.Profile;
        var queryService = uiTools.manipulate.query;
        var multiselectService = uiTools.control.multiselect;

        var getSelectedUser = function() {
            var user = _.findWhere($scope.users, { isSelected: true });
            return user;
        };

        var lastParams = {
            pageNumber: 1,
            pageSize: 15
        };

        $scope.lookup = {
            roles: uiTools.control.multiselect.getSelectChoices(User.defaults.roles)
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

        $scope.query = function(tableState) {
            var params = angular.extend(
                {},             
                queryService.fromStTable(tableState),
                lastParams);
            var promise = User.search(params);
            $scope.loading.addPromise(promise);
            promise.then(function(page) {
                tableState.pagination.start = page.skip;
                tableState.pagination.numberOfPages = page.totalPages;
                lastParams = {};
                $scope.users = page.items;
            });
        };

        $scope.show = function() {
            lastParams = {
                roles: multiselectService.getSelectedItems($scope.lookup.roles),
                pageNumber: 1,
                pageSize: $scope.pageSize,
                searchString: $scope.search
            };
        };

        $scope.add = function() {
            $state.go('admin.userManagement.editUser', { id: 'add' });
        };

        $scope.edit = function(user) {
            var selectedUser = user || getSelectedUser();
            $state.go('admin.userManagement.editUser', { id: selectedUser.userId });
        };

        $scope.unlock = function(user) {
            $modal.open({
                templateUrl: '/Client/app/areas/admin/user-management/user-unlock.html',
                controller: 'userUnlockController',
                resolve: {
                    data: function() {
                        return $q.all({
                            profile: Profile.getOrDefault({ userId: user.userId }),
                            user: user
                        });
                    }
                }
            }).result.then(function() {
                $scope.query({ pagination: {} });
            });
        };
    }
})();
