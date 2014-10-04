(function () {
    'use strict';

    var module = angular.module('vabank.ui');

    module.factory('notificationService', notificationService);

    notificationService.$inject = ['$rootScope', 'toastr', 'localStorageService'];

    function notificationService($rootScope, toastr, localStorage) {

        var notificationSchema = schema({
            type: ['success', 'error', 'info', 'warning'],
            message: String,
        });

        var toastDefaults = {            
          title: 'New notification'
        };

        var scope = $rootScope.$new(true);
        scope.notifications = {};
        var notifications = scope.notifications;
        localStorage.bind(scope, 'notifications', 'notifications');

        var notify = function(notification) {
            if (!notificationSchema(notification) && !notification.isExceptionHandler) {
                var errors = notificationSchema.errors(notification);
                throw new TypeError('Invalid options passed!\n' + JSON.stringify(errors));
            }
            var toast = angular.extend({}, toastDefaults, notification);
            var method = toastr[notification.type];
            if (_.isUndefined(notification.state) || _.isNull(notification.state)) {
                method(toast.message, toast.title);
            } else {
                if (!_.isArray(notifications[toast.state])) {
                    notifications[toast.state] = [];
                }
                notifications[toast.state].push(notification);
            }
        };

        var pop = function(state) {
            var array = notifications[state];
            notifications[state] = [];
            return _.isArray(array) ? array : [];
        };
        
        $rootScope.$on('notificationPushed', function (e, notification) {
            notify(notification);
        });

        return {            
            notify: notify,
            pop: pop
        };
    }

})();