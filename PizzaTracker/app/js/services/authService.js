'use strict';
app.factory('authService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

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
            url: '/api/login/'
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

        //_authentication.user = {
        //    Id: 1,
        //    UserName: "Tom",
        //    Email: "hardcoded@mail.com"
        //};
        
        //deferred.resolve();
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

    //var _login = function (loginData) {

    //    var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

    //    var deferred = $q.defer();

    //    $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

    //        localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName });

    //        _authentication.isAuth = true;
    //        _authentication.userName = loginData.userName;

    //        deferred.resolve(response);

    //    }).error(function (err, status) {
    //        _logOut();
    //        deferred.reject(err);
    //    });

    //    return deferred.promise;

    //};

    //var _logOut = function () {

    //    localStorageService.remove('authorizationData');

    //    _authentication.isAuth = false;
    //    _authentication.userName = "";

    //};

    //var _fillAuthData = function () {

    //    var authData = localStorageService.get('authorizationData');
    //    if (authData) {
    //        _authentication.isAuth = true;
    //        _authentication.userName = authData.userName;
    //    }

    //}

    //authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logout = _logout;
    authServiceFactory.getAuth = _getAuth;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.setAuth = _setAuth;

    return authServiceFactory;
}]);