app.controller('orderController', ['$scope', '$location', '$routeParams', 'resourceService', 'orderService', 'pizzaService',
    function ($scope, $location, $routeParams, resourceService, orderService, pizzaService) {
        $scope.resources = {};
        $scope.confirmModalShown = false;
        $scope.anonUser = {};

        $scope.selectedPizza = {
            Crust: {},
            Sauce: {},
            SauceLevel: {},
            Size: {},
            Toppings: []
        };

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

             $scope.selectedPizza.Crust = lookupById($scope.resources.Crusts, 1);
             $scope.selectedPizza.Sauce = lookupById($scope.resources.Sauces, 1);
             $scope.selectedPizza.SauceLevel = lookupById($scope.resources.SauceLevels, 1);
             $scope.selectedPizza.Size = lookupById($scope.resources.Sizes, 1);
         },
             function (err) {
                 console.log("resource gotten error " + err);
                 $scope.resources = {};
             });

        $scope.createOrder = function () {
            if ($scope.authentication.isAuth) {
                orderService.sendOrder($scope.selectedPizza, $scope.authentication.user.Token)
                    .then(function (response) {
                        console.log("order placed success " + response);
                        $scope.confirmModalShown = false;
                        $location.path('#/');
                        //$scope.$apply();
                    },
                        function (err) {
                            console.log("order placed error " + err);
                            //$scope.resources = {};
                        });
            } else {
                orderService.sendAnonOrder($scope.selectedPizza, $scope.anonUser)
                  .then(function (response) {
                      console.log("order placed success " + response);
                      $scope.confirmModalShown = false;
                      $location.path('#/');
                      //$scope.$apply();
                  },
                      function (err) {
                          console.log("order placed error " + err);
                          //$scope.resources = {};
                      });
            }
        };

        var startingPizzaId = $routeParams.pizzaId;
        if (startingPizzaId) {
            pizzaService.getPizza(startingPizzaId)
           .then(function (response) {
               console.log("pizza gotten2 success " + response);

               $scope.selectedPizza.Crust = lookupById($scope.resources.Crusts, response.Crust.Id);
               $scope.selectedPizza.Sauce = lookupById($scope.resources.Sauces, response.Sauce.Sauce.Id);
               $scope.selectedPizza.SauceLevel = lookupById($scope.resources.SauceLevels, response.Sauce.SauceLevel.Id);
               $scope.selectedPizza.Size = lookupById($scope.resources.Sizes, response.Size.Id);
               $scope.selectedPizza.Toppings = lookupByIds($scope.resources.Toppings, response.Toppings.map(function (v) {
                   return v.Topping;
               }));
           },
            function (err) {
                console.log("pizza gotten2 error " + err);
                //$scope.pizza = null;
            });
        }

    }]);