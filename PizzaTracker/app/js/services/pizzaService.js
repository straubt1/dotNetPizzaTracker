'use strict';
app.factory('pizzaService', ['pizzaAppConfig', '$http', '$q', function (pizzaAppConfig, $http, $q) {

    var pizzaServiceFactory = {};

    pizzaServiceFactory.getPizza = function (id) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: pizzaAppConfig.apiBaseUrl + '/pizza/' + id,
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    return pizzaServiceFactory;
}]);