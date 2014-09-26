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
            $scope.authentication = authService.authentication;
            $location.path('#/');
            //$scope.$apply();
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
        pushService.stop();
        $scope.authentication = authService.authentication;
    };
   
    pushService.callback = function (data) {
        alertify.success(data.Display + "<br/>" + data.Date + "<br/>" + data.Status,
            null,
            function() {
                $location.path('neworder');
            });
        console.log(data);
    };
    

    $scope.authentication = authService.authentication;
    if ($scope.authentication.isAuth) {
        pushService.start();
    }
}]);