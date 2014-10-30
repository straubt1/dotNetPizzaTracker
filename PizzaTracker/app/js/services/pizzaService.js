'use strict';
app.factory('pizzaService', ['pizzaAppConfig', '$http', '$q', '$templateCache', function (pizzaAppConfig, $http, $q, $templateCache) {

    var pizzaServiceFactory = {};

    var _getPizza = function (id) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: pizzaAppConfig.apiBaseUrl + '/pizza/' + id,
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        //$http.jsonp('http://pizzatracker.azurewebsites.net/api' + '/pizza/' + id + '?callback=JSON_CALLBACK', {
        //    //method: 'GET',
        //    //url: 'http://pizzatracker.azurewebsites.net/api' + '/pizza/' + id,
        //    //cache: $templateCache
        //}).success(function (response) {
        //    deferred.resolve(response);
        //}).error(function (err, status) {
        //    deferred.reject(err);
        //});
        return deferred.promise;
    };
    pizzaServiceFactory.getPizza = _getPizza;

    return pizzaServiceFactory;
}]);