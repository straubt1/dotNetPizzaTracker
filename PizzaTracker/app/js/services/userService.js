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

    //var sendMail = function (mail) {
    //    var d = $q.defer();

    //    $http({
    //        method: 'POST',
    //        data: mail,
    //        url: '/api/email'
    //    })
    //     .success(function (data, status, headers) {
    //         d.resolve(data);
    //     })
    //    .error(function (data, status, headers) {
    //        d.reject(data);
    //    });

    //    return d.promise;
    //};

    return {
        getUsers: getUsers,
        removeUser: removeUser,
        editUser: editUser
    };
});