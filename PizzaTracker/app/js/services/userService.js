app.factory('userService', ['pizzaAppConfig', '$http', function (pizzaAppConfig, $http) {
    
    var getUsers = function () {
        return $http({
            method: 'GET',
            url: pizzaAppConfig.apiBaseUrl + '/users'
        });
    };

    var removeUser = function (user) {
        return $http({
            method: 'DELETE',
            url: pizzaAppConfig.apiBaseUrl + '/users/' + user.Id
        });
    };

    var editUser = function (user) {
        return $http({
            method: 'POST',
            data: user,
            url: pizzaAppConfig.apiBaseUrl + '/users'
        });
    };

    return {
        getUsers: getUsers,
        removeUser: removeUser,
        editUser: editUser
    };
}]);