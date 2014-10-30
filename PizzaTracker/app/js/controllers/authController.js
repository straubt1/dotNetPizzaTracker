'use strict';
angular.module('pizzaApp').controller('authController', ['$scope', '$location', 'authService', 'pushService', function ($scope, $location, authService, pushService) {
    authService.getAuth();

    $scope.user = {
        UserName: "tstraub",
        Password: "Test@123"
    };
    $scope.newUser = {};

    $scope.loginerror = null;
    $scope.registererror = null;

    $scope.registerUser = function (user) {
        authService.logout();//just in case
        authService.register(user)
        .then(function (response) {
            $scope.registererror = null;
            $location.path('#/');
            pushService.start($scope.getUserToken());
        },
         function (err) {
             console.log("failed registered user" + err);
             $scope.registererror = err;
         });
    };

    $scope.login = function (user) {
        authService.login(user)
        .then(function (response) {
            $scope.loginerror = null;
            if ($scope.isEmployee()) {
                $location.path('/queue');
            } else {
                $location.path('#/');
            }
            pushService.start($scope.getUserToken());
        },
         function (err) {
             console.log("failed logged in user" + err);
             $scope.loginerror = err;
         });
    };

    $scope.logout = function () {
        authService.logout();
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

    $scope.isLoggedIn = function () {
        if (authService.authentication == null) {
            return false;
        }
        return authService.authentication.isAuth == true;
    }

    $scope.getUserToken = function () {
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

    if (authService.authentication.isAuth) {
        pushService.start($scope.getUserToken());
    }
}]);