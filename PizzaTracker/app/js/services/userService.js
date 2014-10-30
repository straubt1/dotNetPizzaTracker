'use strict';
app.factory('userService', ['pizzaAppConfig', '$http', '$q', function (pizzaAppConfig, $http, $q) {
    var userServiceFactory = {};

    userServiceFactory.getUsers = function (utoken) {
        return $http({
            method: 'GET',
            url: pizzaAppConfig.apiBaseUrl + '/users?token=' + utoken
        });
    };

    userServiceFactory.removeUser = function (utoken, user) {
        var deferred = $q.defer();
        $http({
            method: 'DELETE',
            url: pizzaAppConfig.apiBaseUrl + '/users?token=' + utoken + "&userid=" + user.Id
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    userServiceFactory.editUser = function (utoken, user) {
        var deferred = $q.defer();
        $http({
            method: 'PUT',
            data: user,
            url: pizzaAppConfig.apiBaseUrl + '/users?token=' + utoken
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    userServiceFactory.addUser = function (utoken, user) {
        var deferred = $q.defer();
        $http({
            method: 'POST',
            data: user,
            url: pizzaAppConfig.apiBaseUrl + '/users?token=' + utoken
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };
    return userServiceFactory;
}]);