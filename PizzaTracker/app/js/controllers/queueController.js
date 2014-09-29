app.controller('queueController', ['$scope', '$location', 'queueService', function ($scope, $location, queueService) {
    $scope.queue = {};

    queueService.getQueue()
     .then(function (response) {
         console.log("queue gotten success " + response);
         $scope.queue = response;
     },
         function (err) {
             console.log("queue gotten error " + err);
             $scope.queue = {};
         });

    $scope.updateQueue = function(id) {
        queueService.updateQueue(id).then(function (response) {
            console.log("Queue updated success " + response);
        },
       function (err) {
           console.log("Queue updated " + err);
       });
    };

}]);