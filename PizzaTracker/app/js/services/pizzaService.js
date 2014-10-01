'use strict';
app.factory('pizzaService', ['$http', '$q', function ($http, $q) {

    var pizzaServiceFactory = {};

    var _getPizza = function (id) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: '/api/pizza/' + id
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };
    pizzaServiceFactory.getPizza = _getPizza;

    return pizzaServiceFactory;
}]);