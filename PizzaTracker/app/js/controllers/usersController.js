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
        userService.removeUser(user)
         .success(function (data, status, headers) {
             console.log("success removing user" + data);
             $scope.users.splice($scope.users.indexOf(user), 1);
             $scope.removeModalShown = false;
             $scope.error = null;
         })
         .error(function (data, status, headers) {

         });
    };

    $scope.editUser = function (user) {
        userService.editUser(user)
         .success(function (data, status, headers) {
             console.log("success edit user" + data);
             $scope.editModalShown = false;
             $scope.error = null;
         })
         .error(function (data, status, headers) {
             $scope.error = data;
         });
    };

    $scope.addUser = function (user) {
        console.log("New User: " + user.UserName);
        userService.editUser(user)
         .success(function (data, status, headers) {
             console.log("success adding user" + data);
             $scope.users.splice(1, 0, data);
             $scope.newModalShown = false;
             $scope.error = null;
         })
         .error(function (data, status, headers) {
             $scope.error = data;
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
        count: 4          // count per page
    }, {
        total: 0,           // length of data
       groupBy: 'RoleName',
        getData: function ($defer, params) {
            if ($scope.authentication.isAuth) {
                if (data == null) {
                    userService.getUsers()
                         .success(function (response, status, headers) {
                             params.total(response.length);
                             data = response;
                             var orderedData = $filter('orderBy')(data, $scope.tableParams.orderBy());
                             $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                        })
                         .error(function (response, status, headers) {
                            // $scope.error = data;
                         });

                    //orderService.getOrders($scope.authentication.user.Token)
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