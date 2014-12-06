(function () {
    'use strict';

    angular.module('vabank.webapp').controller('velcomPhoneNoController', velcomPhoneNoController);

    velcomPhoneNoController.$inject = ['$scope', 'uiTools'];

    function velcomPhoneNoController($scope, uiTools) {
        
        _.extend($scope.payment.form, {            
           phoneNo: {
               name: 'Номер телефона',
               value: ''
           } 
        });
    }

})();