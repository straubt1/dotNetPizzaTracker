'use strict';
angular.module('pizzaApp').controller('authController', ['$scope', '$location', 'authService', 'pushService', function ($scope, $location, authService, pushService) {

    authService.getAuth();

    $scope.user = {
        UserName: "tstraub",
        Password: "Test@123"
    };

    $scope.error = null;

    $scope.login = function (user) {
        authService.login(user)
        .then(function (response) {
            console.log("success logged in user" + response);
            $scope.error = null;
            $location.path('#/');
            pushService.UserToken = $scope.getUserToken();
            pushService.start();
        },
         function (err) {
             console.log("failed logged in user" + err);
             $scope.error = err;
         });
    };

    $scope.logout = function () {
        console.log("logout");
        authService.logout();
        pushService.UserToken = null;
        pushService.stop();
        $location.path('#/');
    };

    pushService.callback = function (data) {
        var d = new Date(data.Date);
        alertify.success(d.toLocaleTimeString() + "<br/>" + data.MessageTitle + "<br/>" + data.MessageBody,
            null,
            function () {
                $location.path('neworder');
            });
        console.log(data);
    };

    if (authService.authentication.isAuth) {
        pushService.UserToken = authService.authentication.user.Token;
        pushService.start();
    }

    $scope.isAdmin = function () {
        if (authService.authentication == null || !authService.authentication.isAuth) {
            return false;
        }

        return authService.authentication.user.Role == 'Admin';
    };
    $scope.isEmployee = function () {
        if (authService.authentication == null || !authService.authentication.isAuth) {
            return false;
        }

        return authService.authentication.user.Role == 'Employee' || authService.authentication.user.Role == 'Admin';
    };

    $scope.isLoggedIn = function() {
        if (authService.authentication == null) {
            return false;
        }
        return authService.authentication.isAuth == true;
    }

    $scope.getUserToken= function() {
        if (authService.authentication == null || !authService.authentication.isAuth || authService.authentication.user == null) {
            return null;
        }
        return authService.authentication.user.Token;
    }

    $scope.getUser = function () {
        if (authService.authentication == null || !authService.authentication.isAuth || authService.authentication.user == null) {
            return null;
        }
        return authService.authentication.user;
    }
}]);