'use strict';
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

    var _getQueue = function (userId) {
        var deferred = $q.defer();

        $http({
            method: 'GET',
            url: '/api/queue' //'?id=' + encodeURIComponent(userId)
        }).success(function (response) {
            //localStorageService.set('resources', response);
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    var _updateQueue = function (id) {
        var deferred = $q.defer();

        $http({
            method: 'POST',
            url: '/api/queue/' + id //'?id=' + encodeURIComponent(userId)
        }).success(function (response) {
            //localStorageService.set('resources', response);
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };
    queueServiceFactory.updateQueue = _updateQueue;
    queueServiceFactory.getQueue = _getQueue;
    return queueServiceFactory;
}]);