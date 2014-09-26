'use strict';
app.factory('orderService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var orderServiceFactory = {};

    var _sendOrder = function (pizza) {
        var deferred = $q.defer();
        //var local = localStorageService.get('resources');

        $http({
            method: 'POST',
            data: pizza,
            url: '/api/order'
        }).success(function (response) {
            //orderServiceFactory = response;
            //localStorageService.set('resources', response);
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _getOrders = function (pizza) {
        var deferred = $q.defer();
        //var local = localStorageService.get('resources');

        $http({
            method: 'GET',
            url: '/api/order'
        }).success(function (response) {
            //orderServiceFactory = response;
            //localStorageService.set('resources', response);
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };
    //orderServiceFactory.getResources = _getResources;
    orderServiceFactory.sendOrder = _sendOrder;
    orderServiceFactory.getOrders = _getOrders;
    return orderServiceFactory;
}]);