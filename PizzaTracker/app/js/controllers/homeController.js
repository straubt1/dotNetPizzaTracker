angular.module('pizzaApp').controller('homeController', ['$scope', 'orderService', function ($scope, orderService) {

    $scope.orders = {};
    if ($scope.authentication.isAuth) {
        orderService.getOrders($scope.authentication.user.Token)
        .then(function (response) {
            console.log("Orders gotten");
            $scope.orders = response;
        },
            function (err) {
                console.log("failed to get orders");
                $scope.error = err;
            });
    }
}]);