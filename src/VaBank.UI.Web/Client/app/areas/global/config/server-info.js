(function () {
    'use strict';
    var json = document.getElementById('server-info').text;
    var info = JSON.parse(json);

    angular.module('vabank.webapp').constant('serverInfo', info);
})();