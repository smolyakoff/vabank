(function () {
    'use strict';
    var module = angular.module('vabank.ui', [
        'ui.router',
        'toastr', '' +
        'LocalStorageModule'
    ]);

    module.run(['formForTemplateConfig', main]);

    function main(formForTemplateConfig) {
        formForTemplateConfig.init();
    }

})();