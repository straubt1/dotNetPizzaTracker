'use strict';
app.factory('queueService', ['pizzaAppConfig', '$http', '$q', function (pizzaAppConfig, $http, $q) {
    var queueServiceFactory = {};

    queueServiceFactory.getQueue = function (utoken) {
        var deferred = $q.defer();

        $http({
            method: 'GET',
            url: pizzaAppConfig.apiBaseUrl + '/pizzaqueue?token='+ utoken
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    queueServiceFactory.updateQueue = function (utoken, id, statusId) {
        var deferred = $q.defer();

        $http({
            method: 'POST',
            url: pizzaAppConfig.apiBaseUrl + '/pizzaqueue?token=' + utoken,
            data: JSON.stringify({ Id: id, StatusId: statusId })
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    queueServiceFactory.deleteQueue = function (utoken, id) {
        var deferred = $q.defer();

        $http({
            method: 'DELETE',
            url: pizzaAppConfig.apiBaseUrl + '/pizzaqueue?token=' + utoken + '&orderid=' + id
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    return queueServiceFactory;
}]);