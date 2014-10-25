'use strict';
app.factory('orderService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var orderServiceFactory = {};

    var _sendOrder = function (pizza, userId) {
        var deferred = $q.defer();
        //var local = localStorageService.get('resources');

        pizza.UserToken = userId;
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

    var _sendAnonOrder = function (pizza, userId) {
        var deferred = $q.defer();
        //var local = localStorageService.get('resources');

        pizza.UserToken = userId;
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

    var _getOrders = function (userId) {
        var deferred = $q.defer();
        //var local = localStorageService.get('resources');

        $http({
            method: 'GET',
            url: '/api/order?id=' + encodeURIComponent(userId)
        }).success(function (response) {
            //orderServiceFactory = response;
            //localStorageService.set('resources', response);
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _deleteOrder = function (orderId) {
        var deferred = $q.defer();

        $http({
            method: 'DELETE',
            url: '/api/order/' + orderId 
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };
    //orderServiceFactory.getResources = _getResources;
    orderServiceFactory.sendOrder = _sendOrder;
    orderServiceFactory.sendAnonOrder = _sendAnonOrder;
    orderServiceFactory.getOrders = _getOrders;
    orderServiceFactory.deleteOrder = _deleteOrder;
    return orderServiceFactory;
}]);