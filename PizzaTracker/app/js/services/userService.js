angular.module('pizzaApp').service('userService', function ($http) {
    var getUsers = function () {
        return $http({
            method: 'GET',
            url: '/api/users'
        });
    };

    var removeUser = function (user) {
        return $http({
            method: 'DELETE',
            url: '/api/users/' + user.Id
        });
    };

    var editUser = function (user) {
        return $http({
            method: 'POST',
            data: user,
            url: '/api/users'
        });
    };

    return {
        getUsers: getUsers,
        removeUser: removeUser,
        editUser: editUser
    };
});