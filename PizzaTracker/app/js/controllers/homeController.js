angular.module('pizzaApp').controller('homeController', ['$scope', 'orderService', 'emailService', 'ngTableParams', function ($scope, orderService, emailService, ngTableParams) {

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

    $scope.tableParams = {};

    $scope.shareOrder = function () {
        var subject = $scope.getUser().Name + " wants to share a Pizza with you - Pizza Tracker";
        var message = "<html>Hey!<br/>Check out this pizza <a href='" + $scope.shareModel.order.ShareLink + "'>My Awesome Pizza</a><br/><br/>" +
            $scope.shareModel.message + "</html>";

        emailService.sendEmail($scope.shareModel.email, subject, message)
             .then(function () {
                 alertify.success("Pizza Has Been Shared!",
                     null,
                     null);
                 $scope.shareModel = {
                     order: {},
                     message: "",
                     email: ""
                 };
             },
            function (err) {
                alertify.error("Sorry, there was a problem sending your email!",
                    null,
                    null);
            });
        console.log($scope.shareModel.order.Pizzas[0].Id);
        $scope.shareModalShown = false;
    };

    $scope.getOrders = function () {
        if ($scope.isLoggedIn()) {
            orderService.getOrders($scope.getUserToken())
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
                       //$scope.getOrders();
                       data = null;
                       $scope.tableParams.reload();
                   },
                       function (err) {
                           console.log("failed to remove order");
                       });
            }
        });
    };

    //$scope.getOrders();

    var data = null;

    $scope.tableParams = new ngTableParams({
        page: 1,            // show first page
        count: 4          // count per page
    }, {
        total: 0,           // length of data
        getData: function ($defer, params) {
            if ($scope.isLoggedIn()) {
                if (data == null) {
                    var tok = $scope.getUserToken();
                    orderService.getOrders(tok)
                        .then(function (response) {
                            params.total(response.length);
                            data = response;
                            $scope.orders = data;
                            $defer.resolve(response.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                        },
                            function (err) {
                                console.log("failed to get orders");
                            });
                } else {
                    $defer.resolve(data.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                }
            }
        }
    });
}]);