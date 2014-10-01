app.controller('queueController', ['$scope', '$location', 'queueService', 'resourceService', function ($scope, $location, queueService, resourceService) {
    $scope.queue = {};
    $scope.statuses = {};

    $scope.getQueue = function () {
        queueService.getQueue()
            .then(function (response) {
                console.log("queue gotten success " + response);
                $scope.queue = response;
                for (var i = 0; i < $scope.queue.length; i++) {
                    $scope.queue[i].Status = lookupById($scope.statuses, $scope.queue[i].Status.Id);
                }
            },
                function (err) {
                    console.log("queue gotten error " + err);
                    $scope.queue = {};
                });
    }

    $scope.updateQueue = function (q) {
        queueService.updateQueue(q.Id, q.Status.Id)
            .then(function (response) {
                console.log("Queue updated success " + response);
            },
       function (err) {
           console.log("Queue updated " + err);
       });
    };

    $scope.deleteQueue = function (id) {
        alertify.confirm("Are you sure you want to remove this Pizza from the Queue?", function (e) {
            if (e) {
                queueService.deleteQueue(id)
                    .then(function (response) {
                        console.log("Queue delete success " + response);
                        $scope.getQueue();
                    },
               function (err) {
                   console.log("Queue delete error " + err);
               });
            }
        });
    };

    resourceService.getResources(false)
    .then(function (response) {
        console.log("resource gotten success " + response);
        $scope.statuses = response.Statuses;
    },
        function (err) {
            console.log("resource gotten error " + err);
            $scope.statuses = {};
        });

    $scope.getQueue();
}]);