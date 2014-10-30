'use strict';
app.factory('orderService', ['pizzaAppConfig', '$http', '$q', 'localStorageService', 'authService', 'pushService', function (pizzaAppConfig, $http, $q, localStorageService, authService, pushService) {
    var orderServiceFactory = {};

    orderServiceFactory.sendOrder = function (pizza, notifications, userId) {
        var deferred = $q.defer();
        pizza.UserToken = userId;
        pizza.IsAnon = false;
        pizza.Notifications = notifications;
        $http({
            method: 'POST',
            data: pizza,
            url: pizzaAppConfig.apiBaseUrl + '/order'
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    orderServiceFactory.sendAnonOrder = function (pizza, notifications, anonUser) {
        var deferred = $q.defer();
        pizza.UserToken = anonUser.Email;
        pizza.IsAnon = true;
        pizza.Notifications = notifications;
        $http({
            method: 'POST',
            data: pizza,
            url: pizzaAppConfig.apiBaseUrl + '/order'
        }).success(function (response) {
            authService.setAuth(response.AnonUser, true);
            pushService.start(response.AnonUser.Token);
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    orderServiceFactory.getOrders = function (userId) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: pizzaAppConfig.apiBaseUrl + '/order?token=' + encodeURIComponent(userId)
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    orderServiceFactory.deleteOrder = function (utoken, orderId) {
        var deferred = $q.defer();
        $http({
            method: 'DELETE',
            url: pizzaAppConfig.apiBaseUrl + '/order?token=' + utoken + '&orderid='+ orderId
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    return orderServiceFactory;
}]);