//$(document).foundation();

var app = angular.module("pizzaApp", ['ngRoute', 'ngAnimate', 'ngCookies', 'angular-loading-bar', 'LocalStorageModule']);

app.constant('pizzaAppConfig',
{
    notificationInterval: 1000,//time between polling
    notificationIsListening: false//are we listening
});
app.config(function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: "templates/home.html",
            controller: 'homeController'
        })
        .when('/login', {
            templateUrl: "templates/login.html",
            controller: 'authController'
        })
        .when('/users', {
            templateUrl: "templates/users.html",
            controller: 'usersController'
        })
        .when('/neworder/:pizzaId', {
            templateUrl: "templates/neworder.html",
            controller: 'orderController'
        })
        .when('/neworder', {
            templateUrl: "templates/neworder.html",
            controller: 'orderController'
        })
        .when('/queue', {
            templateUrl: "templates/queue.html",
            controller: 'queueController'
        })
        .when('/pizza/:pizzaId', {
            templateUrl: "templates/pizza.html",
            controller: 'pizzaController'
        })
        .otherwise({
            redirectTo: '/' //redirect to home route
        });
});

function lookupById(arr, id) {
    var n = $.grep(arr, function (a) {
        return a.Id == id;
    });
    return (n && n.length > 0) ? n[0] : null;
}

function lookupByIds(arr, idArr) {
    var list = [];
    for (var i = 0; i < idArr.length; i++) {
        list.push(lookupById(arr, idArr[i].Id));
    }
    return list;
}