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
        .when('/neworder', {
            templateUrl: "templates/neworder.html",
            controller: 'orderController'
        })
        .when('/queue', {
            templateUrl: "templates/queue.html",
            controller: 'queueController'
        })
        .otherwise({
            redirectTo: '/' //redirect to home route
        });
});