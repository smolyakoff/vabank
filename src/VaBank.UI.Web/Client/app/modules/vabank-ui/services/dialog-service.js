(function() {
    'use strict';

    angular.module('vabank.ui').factory('dialogService', dialogService);

    dialogService.$inject = ['$modal'];

    function dialogService($modal) {

        var confirmationOptionsSchema = schema({
            message: String,
            title: String
        });

        var confirmationController = ['$scope', 'options', function($scope, options) {
            $scope.options = options;
        }];
        
        var confirmation = function (options) {
            if (!confirmationOptionsSchema(options)) {
                var errors = confirmationOptionsSchema.errors(options);
                throw new TypeError('Invalid options passed!\n' + JSON.stringify(errors));
            }
            return $modal.open({                
                controller: confirmationController,
                resolve: {
                    options: function () { return options; }
                },
                backdrop: 'static',
                templateUrl: '/Client/app/modules/vabank-ui/templates/vb-confirmation-dialog.html'
            });
        };

        return {            
            confirmation: confirmation
        };
    }

})();