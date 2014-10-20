(function () {
    'use strict';

    var module = angular.module('vabank.ui');

    module.factory('notificationService', notificationService);

    notificationService.$inject = ['$rootScope', '$state', 'toastr', 'localStorageService'];

    function notificationService($rootScope, $state, toastr, localStorage) {

        var notificationSchema = schema({
            type: ['success', 'error', 'info', 'warning'],
            message: String,
        });

        var getDefaultTitle = function(type) {
            var titles = {
                info: 'Новое уведомление',
                warning: 'Предупреждение',
                error: 'Произошла ошибка',
                success: 'Успешная операция'
            };
            return titles[type];
        };

        var toastDefaults = {            
            type: 'info'
        };

        var scope = $rootScope.$new(true);
        scope.notifications = {};
        var notifications = scope.notifications;
        localStorage.bind(scope, 'notifications', 'notifications');
        var notify = function (notification) {
            var toast = angular.extend({}, toastDefaults, notification);
            toast.title = toast.title || getDefaultTitle(toast.type);
            if (!notificationSchema(toast) && !notification.isExceptionHandler) {
                var errors = notificationSchema.errors(toast);
                throw new TypeError('Invalid options passed!\n' + JSON.stringify(errors));
            }
            var method = toastr[toast.type];
            if (_.isUndefined(toast.state) || _.isNull(toast.state) || $state.current.name === toast.state) {
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