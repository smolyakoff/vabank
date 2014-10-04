(function () {
    'use strict';

    var app = angular.module('vabank.webapp');
    app.controller('exceptionDetailsController', exceptionDetails);

    exceptionDetails.$inject = ['$scope', 'data'];

    function exceptionDetails($scope, data) {
        $scope.log = data;
    }

})();
