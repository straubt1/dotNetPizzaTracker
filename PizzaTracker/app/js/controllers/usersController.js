angular.module('pizzaApp').controller('usersController', ['$scope', '$filter', 'userService', 'ngTableParams', function ($scope, $filter, userService, ngTableParams) {
    $scope.users = {};
    $scope.currentUser = {};
    $scope.newUser = {};
    $scope.error = null;

    $scope.removeModalShown = false;
    $scope.editModalShown = false;
    $scope.newModalShown = false;

    $scope.toggleRemoveModal = function (user) {
        $scope.removeModalShown = !$scope.removeModalShown;
        $scope.currentUser = user;
    };
    $scope.toggleEditModal = function (user) {
        $scope.editModalShown = !$scope.editModalShown;
        $scope.currentUser = user;
    };
    $scope.toggleNewModal = function () {
        $scope.newModalShown = !$scope.newModalShown;
        $scope.newUser = {};
    };


    $scope.removeUser = function (user) {
        alertify.confirm("Are you sure you want to remove this user <br/>from the system?", function (e) {
            if (e) {
                userService.removeUser($scope.getUserToken(), user)
                     .then(function (response) {
                         data.splice(data.indexOf(user), 1);
                         $scope.tableParams.reload();
                         $scope.error = null;
                     },
                    function (err) {
                        console.log("user not removed error " + err);
                        $scope.error = err;
                    });
            }
        });
    };

    $scope.editUser = function (user) {
        userService.editUser($scope.getUserToken(), user)
            .then(function (response) {
                $scope.editModalShown = false;
            },
            function (err) {
                console.log("user not updated error " + err);
                $scope.error = err;
            });
    };

    $scope.addUser = function (user) {
        userService.addUser($scope.getUserToken(), user)
            .then(function (response) {
                data.splice(1, 0, response);
                $scope.tableParams.reload();
                $scope.newModalShown = false;
                $scope.error = null;
            },
            function (err) {
                console.log("user not updated error " + err);
                $scope.error = err;
            });
    };

    //userService.getUsers()
    //  .success(function (data, status, headers) {
    //      $scope.users = data;
    //  })
    //  .error(function (data, status, headers) {
    //      $scope.error = data;
    //  });


    var data = null;

    $scope.tableParams = new ngTableParams({
        page: 1,            // show first page
        count: 10          // count per page
    }, {
        total: 0,           // length of data
        groupBy: 'RoleName',
        getData: function ($defer, params) {
            if ($scope.isLoggedIn()) {
                if (data == null) {
                    userService.getUsers($scope.getUserToken())
                         .success(function (response, status, headers) {
                             params.total(response.length);
                             data = response;
                             var orderedData = $filter('orderBy')(data, $scope.tableParams.orderBy());
                             $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         })
                         .error(function (response, status, headers) {
                             // $scope.error = data;
                         });

                    //orderService.getOrders($scope.getUserToken())
                    //    .then(function (response) {
                    //        params.total(response.length);
                    //        data = response;
                    //        $defer.resolve(response.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                    //    },
                    //        function (err) {
                    //            console.log("failed to get orders");
                    //        });
                } else {
                    $defer.resolve(data.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                }
            }
        }
    });
}]);