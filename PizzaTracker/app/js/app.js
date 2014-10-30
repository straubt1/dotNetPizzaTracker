//$(document).foundation();

var app = angular.module("pizzaApp", ['ngRoute', 'ngAnimate', 'ngCookies', 'angular-loading-bar', 'LocalStorageModule', 'ngTable']);

app.constant('pizzaAppConfig',
{
    //apiBaseUrl: 'http://pizzatracker.azurewebsites.net/api',
    apiBaseUrl: 'http://localhost:60027/api',
    notificationInterval: 1000,//time between polling
    notificationIsListening: true//are we listening
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

function elaspedTime(date1, date2) {
    var diff = date2.addHours(-4).getTime() - date1.getTime();
    var diffTotal = diff;
    var days = Math.floor(diff / (1000 * 60 * 60 * 24));
    var totaldays = Math.floor(diffTotal / (1000 * 60 * 60 * 24));
    diff -= days * (1000 * 60 * 60 * 24);

    var hours = Math.floor(diff / (1000 * 60 * 60));
    var totalhours = Math.floor(diffTotal / (1000 * 60 * 60));
    diff -= hours * (1000 * 60 * 60);

    var mins = Math.floor(diff / (1000 * 60));
    var totalmins = Math.floor(diffTotal / (1000 * 60));
    diff -= mins * (1000 * 60);

    var seconds = Math.floor(diff / (1000));
    var totalseconds = Math.floor(diffTotal / (1000));

    return {
        days: days, hours: hours, minutes: mins, seconds: seconds,
        totaldays: totaldays, totalhours: totalhours, totalminutes: totalmins, totalseconds: totalseconds
    };
}

Date.prototype.addHours = function (h) {
    this.setHours(this.getHours() + h);
    return this;
}

function getElapsedColor(totalminutes) {
    var MAX = 60;
    totalminutes = totalminutes > MAX ? MAX : totalminutes;//cap at MAX minutes
    var color = "#FF";
    var value = 255 - totalminutes * (255 / MAX);//integer division
    var hex = parseInt(value).toString(16);
    hex = hex.length == 1 ? "0" + hex : hex;
    return "#FF" + hex + hex;
}