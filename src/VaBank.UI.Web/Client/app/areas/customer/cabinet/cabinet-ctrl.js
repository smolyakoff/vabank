(function () {
    'use strict';

    angular.module('vabank.webapp').controller('cabinetController', cabinetController);

    cabinetController.$inject = ['$scope', 'uiTools', 'data'];

    function cabinetController($scope, uiTools, data) {
        $scope.userProfile = data.profile;

        $scope.noCards = data.cards.length === 0;

        if ($scope.noCards) {
            uiTools.notify({
                title: 'Нет доступных карт',
                message: 'У Вас нет доступных карт для онлайн-банкинга. Обратитесь в техническую поддержку.',
                type: 'warning'
            });
        }
    }

})();