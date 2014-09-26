angular.module('pizzaApp').controller('usersController', ['$scope', 'userService', function ($scope, userService) {
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

    userService.getUsers()
      .success(function (data, status, headers) {
          $scope.users = data;
      })
      .error(function (data, status, headers) {
          $scope.error = data;
      });
}]);