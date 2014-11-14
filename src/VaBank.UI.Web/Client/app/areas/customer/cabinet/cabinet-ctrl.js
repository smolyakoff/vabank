(function () {
    'use strict';

    angular.module('vabank.webapp').controller('cabinetController', cabinetController);

    cabinetController.$inject = ['$scope', 'myCardsService', 'data'];

    function cabinetController($scope, myCardsService, data) {
        $scope.userProfile = data.profile;

    }

})();