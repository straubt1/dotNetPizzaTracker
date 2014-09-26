app.controller('orderController', ['$scope', '$location', 'resourceService', 'orderService', function ($scope, $location, resourceService, orderService) {
    $scope.resources = {};
    $scope.confirmModalShown = false;

    $scope.selectedPizza = {
        Crust: {},
        Sauce: {},
        Size: {},
        Toppings: []
    };




    $scope.selectCrust = {};
    $scope.selectSauce = {};
    $scope.selectSize = {};
    $scope.selectToppings = [];
    $scope.toggleTopping = function (crust) {
        var idx = $scope.selectedPizza.Toppings.indexOf(crust);
        if (idx > -1) {
            $scope.selectedPizza.Toppings.splice(idx, 1);
        }
        else {
            $scope.selectedPizza.Toppings.push(crust);
        }
    };

    $scope.toggleConfirmModal = function () {
        $scope.confirmModalShown = !$scope.confirmModalShown;
    };

    resourceService.getResources(false)
     .then(function (response) {
         console.log("resource gotten success " + response);
         $scope.resources = response;

         $scope.selectedPizza.Crust = response.Crusts[0];
         $scope.selectedPizza.Sauce = response.Sauces[0];
         $scope.selectedPizza.Size = response.Sizes[0];
        },
         function (err) {
             console.log("resource gotten error " + err);
             $scope.resources = {};
         });

    $scope.createOrder = function() {
        console.log("new order placed");
        //var pizza = {
        //    Crust: $scope.selectCrust,
        //    Sauce: $scope.selectSauce,
        //    Size: $scope.selectSize,
        //    Toppings: $scope.selectToppings
        //};
        orderService.sendOrder($scope.selectedPizza)
        .then(function (response) {
            console.log("order placed success " + response);
            $scope.confirmModalShown = false;
            $location.path('#/');
            $scope.$apply();
        },
         function (err) {
             console.log("order placed error " + err);
             //$scope.resources = {};
         });

    };

}]);