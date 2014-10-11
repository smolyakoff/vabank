angular.module('vabank.ui').directive('stForcePipe', function () {
    return {
        require: '^stTable',
        priority: 100,
        template: '',
        link: function (scope, element, attr, ctrl) {
            element.bind('click', function() {
                ctrl.pipe();
            });
        }
    };
});