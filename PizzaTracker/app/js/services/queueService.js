﻿'use strict';
app.factory('queueService', ['$http', '$q', function ($http, $q) {

    var queueServiceFactory = {};

    //var _sendOrder = function (pizza, userId) {
    //    var deferred = $q.defer();
    //    //var local = localStorageService.get('resources');

    //    pizza.UserToken = userId;
    //    $http({
    //        method: 'POST',
    //        data: pizza,
    //        url: '/api/order'
    //    }).success(function (response) {
    //        //orderServiceFactory = response;
    //        //localStorageService.set('resources', response);
    //        deferred.resolve(response);
    //    }).error(function (err, status) {
    //        deferred.reject(err);
    //    });
    //    return deferred.promise;
    //};

    var _getQueue = function () {
        var deferred = $q.defer();

        $http({
            method: 'GET',
            url: '/api/pizzaqueue'
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _updateQueue = function (id, statusId) {
        var deferred = $q.defer();

        $http({
            method: 'POST',
            url: '/api/pizzaqueue',
            data: JSON.stringify({ Id: id, StatusId: statusId })
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _deleteQueue = function (id) {
        var deferred = $q.defer();

        $http({
            method: 'DELETE',
            url: '/api/pizzaqueue/' + id
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    queueServiceFactory.updateQueue = _updateQueue;
    queueServiceFactory.getQueue = _getQueue;
    queueServiceFactory.deleteQueue = _deleteQueue;
    return queueServiceFactory;
}]);