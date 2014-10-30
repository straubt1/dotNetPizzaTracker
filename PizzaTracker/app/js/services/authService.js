'use strict';
app.factory('authService', ['pizzaAppConfig', '$http', '$q', 'localStorageService', function (pizzaAppConfig, $http, $q, localStorageService) {
    var authServiceFactory = {};

    authServiceFactory.authentication = {
        user: {},
        isAuth: false
    };

    authServiceFactory.setAuth = function (u, a) {
        authServiceFactory.authentication.user = u;
        authServiceFactory.authentication.isAuth = a;
        localStorageService.set('user', authServiceFactory.authentication.user);
    };

    authServiceFactory.getAuth = function () {
        var cookie = localStorageService.get('user');
        if (cookie && cookie.Expiration &&
            new Date(cookie.Expiration) > new Date()) {
            //found and not believed to be expired
            authServiceFactory.authentication.user = cookie;
            authServiceFactory.authentication.isAuth = true;
        } else {
            authServiceFactory.authentication.user = {};
            authServiceFactory.authentication.isAuth = false;
        }
    };

    authServiceFactory.register = function (user) {
        var deferred = $q.defer();
        $http({
            method: 'POST',
            data: user,
            url: pizzaAppConfig.apiBaseUrl + '/register/'
        }).success(function (response) {
            authServiceFactory.authentication.user = response;
            authServiceFactory.authentication.isAuth = true;
            localStorageService.set('user', authServiceFactory.authentication.user);
            deferred.resolve(response);
        }).error(function (err, status) {
            authServiceFactory.authentication.user = {};
            authServiceFactory.authentication.isAuth = false;
            localStorageService.remove('user');
            deferred.reject(err);
        });

        return deferred.promise;
    };

    authServiceFactory.login = function (user) {
        var deferred = $q.defer();
        $http({
            method: 'POST',
            data: user,
            url: pizzaAppConfig.apiBaseUrl + '/login/'
        }).success(function (response) {
            authServiceFactory.authentication.user = response;
            authServiceFactory.authentication.isAuth = true;
            localStorageService.set('user', authServiceFactory.authentication.user);
            deferred.resolve(response);
        }).error(function (err, status) {
            authServiceFactory.authentication.user = {};
            authServiceFactory.authentication.isAuth = false;
            localStorageService.remove('user');
            deferred.reject(err);
        });

        return deferred.promise;
    };

    authServiceFactory.logout = function () {
        localStorageService.remove('user');
        authServiceFactory.authentication = {
            user: {},
            isAuth: false
        };
    };

    authServiceFactory.authentication = authServiceFactory.authentication;
    return authServiceFactory;
}]);