(function () {
    'use strict';
    var module = angular.module('vabank.ui', [
        'ui.router',
        'toastr', 
        'LocalStorageModule',
        'smart-table'
    ]);

    module.run(['formForTemplateConfig', main]);

    function main(formForTemplateConfig) {
        formForTemplateConfig.init();
    }

})();