(function() {
    'use strict';

    angular.module('vabank.ui').directive('disableAnimate', disableNgAnimate)
    
    function disableNgAnimate($animate) {
        return {
            restrict: 'A',
            link: function (scope, element) {
                $animate.enabled(false, element);
            }
        };
    }
    disableNgAnimate.$inject = ['$animate'];
})();