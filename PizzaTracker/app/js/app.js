//$(document).foundation();

var app = angular.module("pizzaApp", ['ngRoute', 'ngAnimate', 'ngCookies', 'angular-loading-bar', 'LocalStorageModule']);

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
        .otherwise({
            redirectTo: '/' //redirect to home route
        });
});