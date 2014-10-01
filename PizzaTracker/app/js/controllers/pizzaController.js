app.controller('pizzaController', ['$scope', '$routeParams', 'pizzaService', function ($scope, $routeParams, pizzaService) {
    $scope.pizzaId = $routeParams.pizzaId;
    $scope.pizza = null;

    $scope.getPizza = function (id) {
        pizzaService.getPizza(id)
        .then(function (response) {
            console.log("pizza gotten success " + response);
                $scope.pizza = response;
            },
         function (err) {
             console.log("pizza gotten error " + err);
             $scope.pizza = null;
         });
    };

    $scope.getPizza($scope.pizzaId);
}]);