(function () {
    'use strict';

    angular
        .module('vabank.ui')
        .directive('vbConfirm', vbConfirm);

    vbConfirm.$inject = ['$timeout', 'dialogService'];

    function vbConfirm($timeout, dialogService) {

        var link = function (scope, element, attrs) {
            element.bind('click', function(e) {
                dialogService.confirmation({
                    message: scope.vbConfirm,
                    title: scope.vbConfirmTitle || 'Необходимо подтверждение действия'
                }).result.then(function (isConfirmed) {
                    if (isConfirmed) {
                        scope.vbConfirmClick();
                    }
                });
            });
        };

        var directive = {
            link: link,
            restrict: 'A',
            scope: {
                vbConfirmClick: '&',
                vbConfirm: '@',
                vbConfirmTitle: '@'
            }
        };
        return directive;
    }

})();