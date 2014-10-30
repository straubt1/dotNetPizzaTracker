'use strict';
app.factory('emailService', ['pizzaAppConfig', '$http', '$q', function (pizzaAppConfig, $http, $q) {
    var emailServiceFactory = {};

    emailServiceFactory.sendEmail = function (to, user, link, message, orderid) {
        var d = $q.defer();

        $http({
            method: 'POST',
            data: { to: to, user: user, link: link, message: message, orderid: orderid },
            url: pizzaAppConfig.apiBaseUrl + '/email'
        })
         .success(function (data, status, headers) {
             d.resolve(data);
         })
        .error(function (data, status, headers) {
            d.reject(data);
        });

        return d.promise;
    };

    return emailServiceFactory;
}]);