angular.module('pizzaApp').controller('homeController', ['$scope', 'orderService', function ($scope, orderService) {

    $scope.orders = {};
    $scope.shareModalShown = false;
    $scope.shareModel = {
        order: {},
        message: "",
        email: ""
    };

    $scope.toggleShareModal = function (o) {
        $scope.shareModel.order = o;
        $scope.shareModalShown = !$scope.shareModalShown;
    };

    $scope.shareOrder = function() {

        console.log($scope.shareModel.order.Pizzas[0].Id);
        $scope.shareModalShown = false;
    };

    $scope.getOrders = function () {
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
    }

    $scope.removeOrder = function (id) {
        alertify.confirm("Are you sure you want to remove this Pizza <br/>from your recent Orders?", function (e) {
            if (e) {
                orderService.deleteOrder(id)
                   .then(function (response) {
                       console.log("Order removed");
                       $scope.getOrders();
                   },
                       function (err) {
                           console.log("failed to remove order");
                       });
            }
        });
    };

    $scope.getOrders();
}]);