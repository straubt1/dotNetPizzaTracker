﻿app.factory('emailService', ['$http', '$q', function ($http, $q) {
    var sendMail = function (to, subject, message) {
        var d = $q.defer();

        $http({
            method: 'POST',
            data: { to: to, subject: subject, message: message },
            url: '/api/email'
        })
         .success(function (data, status, headers) {
             d.resolve(data);
         })
        .error(function (data, status, headers) {
            d.reject(data);
        });

        return d.promise;
    };

    return {
        sendEmail: sendMail
    };
}]);