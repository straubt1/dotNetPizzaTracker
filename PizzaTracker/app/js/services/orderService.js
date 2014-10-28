'use strict';
app.factory('orderService', ['$http', '$q', 'localStorageService', 'authService', 'pushService', function ($http, $q, localStorageService, authService, pushService) {

    var orderServiceFactory = {};

    var _sendOrder = function (pizza, notifications, userId) {
        var deferred = $q.defer();
        //var local = localStorageService.get('resources');

        pizza.UserToken = userId;
        pizza.IsAnon = false;
        pizza.Notifications = notifications;
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

    var _sendAnonOrder = function (pizza, notifications, anonUser) {
        var deferred = $q.defer();
        //var local = localStorageService.get('resources');

        pizza.UserToken = anonUser.Email;
        pizza.IsAnon = true;
        pizza.Notifications = notifications;
        $http({
            method: 'POST',
            data: pizza,
            url: '/api/order'
        }).success(function (response) {
            authService.setAuth(response.AnonUser, true);
            pushService.UserToken = response.AnonUser.Token;
            pushService.start();
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