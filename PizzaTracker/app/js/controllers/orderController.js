app.controller('orderController', ['$scope', '$location', '$routeParams', 'resourceService', 'orderService', 'pizzaService',
    function ($scope, $location, $routeParams, resourceService, orderService, pizzaService) {
        $scope.resources = {};
        $scope.confirmModalShown = false;
        $scope.anonUser = {};
        $scope.existing = false;
        $scope.existingDate = null;
        $scope.existingOrderBy = null;

        $scope.selectedPizza = {
            Crust: {},
            Sauce: {},
            SauceLevel: {},
            Size: {},
            Toppings: []
        };

        $scope.orderNotifications = {
            Email: true,
            Push: true,
            Text: true
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
            if ($scope.isLoggedIn()) {
                orderService.sendOrder($scope.selectedPizza, $scope.orderNotifications, $scope.getUserToken())
                    .then(function (response) {
                        console.log("order placed success " + response);
                        $scope.confirmModalShown = false;
                        $location.path('#/');
                    },
                        function (err) {
                            console.log("order placed error " + err);
                        });
            } else {
                orderService.sendAnonOrder($scope.selectedPizza, $scope.orderNotifications, $scope.anonUser)
                  .then(function (response) {
                      console.log("order placed success " + response);
                      $scope.confirmModalShown = false;
                      $location.path('#/');
                  },
                      function (err) {
                          console.log("order placed error " + err);
                      });
            }
        };

        var startingPizzaId = $routeParams.pizzaId;
        if (startingPizzaId) {
            pizzaService.getPizza(startingPizzaId)
           .then(function (response) {
               console.log("pizza gotten2 success " + response);
               $scope.existing = true;
               $scope.existingDate = response.Date;
               $scope.existingOrderBy = response.UserName;

               $scope.selectedPizza.Crust = lookupById($scope.resources.Crusts, response.Pizza.Crust.Id);
               $scope.selectedPizza.Sauce = lookupById($scope.resources.Sauces, response.Pizza.Sauce.Sauce.Id);
               $scope.selectedPizza.SauceLevel = lookupById($scope.resources.SauceLevels, response.Pizza.Sauce.SauceLevel.Id);
               $scope.selectedPizza.Size = lookupById($scope.resources.Sizes, response.Pizza.Size.Id);
               $scope.selectedPizza.Toppings = lookupByIds($scope.resources.Toppings, response.Pizza.Toppings.map(function (v) {
                   return v.Topping;
               }));
           },
            function (err) {
                console.log("pizza gotten2 error " + err);
                //$scope.pizza = null;
            });
        }

    }]);