app.controller('queueController', ['$scope', '$location', 'queueService', 'resourceService', function ($scope, $location, queueService, resourceService) {
    $scope.queue = {};

    $scope.getQueue = function () {
        queueService.getQueue()
            .then(function (response) {
                console.log("queue gotten success " + response);
                $scope.queue = response;
                for (var i = 0; i < $scope.queue.length; i++) {
                    $scope.queue[i].Statuses = $scope.resources.Statuses;
                    $scope.queue[i].Status = $.grep($scope.queue[i].Statuses, function (e) { return e.Id == $scope.queue[i].Status.Id; });
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
        queueService.deleteQueue(id)
            .then(function (response) {
                console.log("Queue delete success " + response);
                $scope.getQueue();
            },
       function (err) {
           console.log("Queue delete error " + err);
       });
    };

    resourceService.getResources(false)
    .then(function (response) {
        console.log("resource gotten success " + response);
        $scope.resources = response;
    },
        function (err) {
            console.log("resource gotten error " + err);
            $scope.resources = {};
        });

    $scope.getQueue();
}]);