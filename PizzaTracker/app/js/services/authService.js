'use strict';
app.factory('authService', ['pizzaAppConfig', '$http', '$q', 'localStorageService', function (pizzaAppConfig, $http, $q, localStorageService) {

    var authServiceFactory = {};

    var _authentication = {
        user: {},
        isAuth: false
    };

    var _setAuth = function(u, a) {
        _authentication.user = u;
        _authentication.isAuth = a;
        localStorageService.set('user', _authentication.user);
    };

    var _getAuth = function () {
        var cookie = localStorageService.get('user');
        if (cookie && cookie.Expiration && 
            new Date(cookie.Expiration) > new Date()) {
            //found and not believed to be expired
            _authentication.user = cookie;
            _authentication.isAuth = true;
        } else {
            _authentication.user = {};
            _authentication.isAuth = false;
        }
        authServiceFactory.authentication = _authentication;
    };

    //var _saveRegistration = function (registration) {

    //    _logOut();

    //    return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
    //        return response;
    //    });

    //};

    var _login = function (user) {
        var deferred = $q.defer();
        $http({
            method: 'POST',
            data: user,
            url: pizzaAppConfig.apiBaseUrl + '/login/'
        }).success(function (response) {
            _authentication.user = response;
            _authentication.isAuth = true;
            localStorageService.set('user', _authentication.user);
            deferred.resolve(response);
        }).error(function (err, status) {
            _authentication.user = {};
            _authentication.isAuth = false;
            localStorageService.remove('user');
            deferred.reject(err);
        });
        
        return deferred.promise;
    };
    var _logout = function () {
        localStorageService.remove('user');
        _authentication = {
            user: {},
            isAuth: false
        };
        authServiceFactory.authentication = _authentication;
    };

    //authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logout = _logout;
    authServiceFactory.getAuth = _getAuth;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.setAuth = _setAuth;

    return authServiceFactory;
}]);