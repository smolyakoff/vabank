(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .controller('editUserController', editUserController);

    editUserController.$inject = ['$scope', '$q', '$state', '$stateParams', 'userManagementService', 'data']; 

    function editUserController($scope, $q, $state, $stateParams, userManager, data) {

        $scope.isEdit = $stateParams.id !== 'add';
        $scope.lookup = {
            roles: _.map(userManager.User.defaults.roles, function (x) {
                return { value: x, label: x };
            })
        };
        $scope.userForm = data;

        $scope.validationRules = {
            userName: {
                 required: true
            },
            password: {
                required: function(value) {
                    return !$scope.isEdit;
                }
            },
            passwordConfirmation: {
                custom: function (value, model) {
                    var deferred = $q.defer();
                    if (!$scope.isEdit || $scope.userForm.changePassword) {
                        if (value !== model.password) {
                            deferred.reject('Пароли должны совпадать');
                        } else {
                            deferred.resolve();
                        }
                    } else {
                        deferred.resolve();
                    }
                    return deferred.promise;
                },
            },
            profile: {
              email: {
                  required: true,
                  type: 'email'
              },
              firstName: {
                  required: true
              },
              lastName: {
                  required: true
              },
              phoneNumber: {
                  required: true
              },
              secretPhrase: {
                  required: true
              }
            }
        };
    }
})();
