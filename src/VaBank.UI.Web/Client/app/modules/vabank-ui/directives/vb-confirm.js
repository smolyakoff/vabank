(function () {
    'use strict';

    angular
        .module('vabank.ui')
        .directive('vbConfirm', vbConfirm);

    vbConfirm.$inject = ['dialogService'];

    function vbConfirm(dialogService) {

        var link = function (scope, element, attrs) {
            element.bind('click', function() {
                dialogService.confirmation({
                    message: scope.vbConfirm,
                    title: scope.vbConfirmTitle || 'Необходимо подтверждение действия'
                }).result.then(function (isConfirmed) {
                    if (isConfirmed) {
                        scope.$parent.$eval(attrs.ngClick);
                    }
                });
            });
        };

        var directive = {
            priority: 1,
            terminal: true,
            link: link,
            restrict: 'A',
            scope: {
                vbConfirm: '@',
                vbConfirmTitle: '@'
            }
        };
        return directive;
    }

})();