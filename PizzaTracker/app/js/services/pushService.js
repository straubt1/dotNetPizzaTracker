'use strict';
app.factory('pushService', ['pizzaAppConfig', '$http', function (pizzaAppConfig, $http) {
    var pushServiceFactory = {};

    pushServiceFactory.running = false;
    pushServiceFactory.callback = null;
    pushServiceFactory.UserToken = null;
    pushServiceFactory.failedCount = 0;

    var run = function () {
        getMessage();
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
            url: pizzaAppConfig.apiBaseUrl + '/message?token=' + encodeURIComponent(pushServiceFactory.UserToken),
            ignoreLoadingBar: true
        })
                .success(function (data, status, headers, config) {
                    //console.log("  push service success: " + data);
                    pushServiceFactory.failedCount = 0;
                    if (data && data != "null") {
                        pushServiceFactory.callback(data);
                    }
                    setTimeout(getMessage, pizzaAppConfig.notificationInterval);
                })
                .error(function (data, status, headers, config) {
                    pushServiceFactory.failedCount++;
                    if (pushServiceFactory.failedCount > 10) {
                        pushServiceFactory.stop();
                    }
                    console.log("  push service failed: " + data);
                    setTimeout(getMessage, pizzaAppConfig.notificationInterval);
                });
    };

    pushServiceFactory.start = function (utoken) {
        if (pizzaAppConfig.notificationIsListening && !pushServiceFactory.running) {
            pushServiceFactory.UserToken = utoken;
            pushServiceFactory.running = true;
            pushServiceFactory.failedCount = 0;
            run();
        }
    };
    pushServiceFactory.stop = function () {
        pushServiceFactory.UserToken = null;
        pushServiceFactory.running = false;
    };

    return pushServiceFactory;
}]);