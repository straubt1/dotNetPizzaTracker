'use strict';
app.factory('pushService', ['pizzaAppConfig', '$http', '$q', function (pizzaAppConfig, $http, $q) {
    var pushServiceFactory = {};

    pushServiceFactory.running = false;
    pushServiceFactory.callback = null;
    pushServiceFactory.UserToken = null;

    var run = function () {
        // var i = 0;
        //while (pushServiceFactory.running) {
        //    console.log("running push service: " + i++);
        getMessage();

        //pushServiceFactory.running = false;
        // }
    };

    var getMessage = function () {
        if (!pushServiceFactory.running) {
            return;
        }
        if (!pushServiceFactory.UserToken) {
            setTimeout(getMessage, pizzaAppConfig.notificationInterval);
            return;
        }
        $http({
            method: 'GET',
            url: '/api/message?id=' + encodeURIComponent(pushServiceFactory.UserToken),
            ignoreLoadingBar: true
        })
                .success(function (data, status, headers, config) {
                    console.log("  push service success: " + data);
                    if (data && data != "null") {
                        pushServiceFactory.callback(data);
                    }
                    setTimeout(getMessage, pizzaAppConfig.notificationInterval);
                })
                .error(function (data, status, headers, config) {
                    console.log("  push service failed: " + data);
                    setTimeout(getMessage, pizzaAppConfig.notificationInterval);
                });
    };

    var _start = function () {
        pushServiceFactory.running = true;
        if (pizzaAppConfig.notificationIsListening)
        { run(); }
    }
    var _stop = function () {
        pushServiceFactory.running = false;
    }

    pushServiceFactory.start = _start;
    pushServiceFactory.stop = _stop;

    return pushServiceFactory;
}]);