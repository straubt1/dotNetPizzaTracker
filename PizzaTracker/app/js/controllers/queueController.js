app.controller('queueController', ['$scope', '$location', 'queueService', 'resourceService', 'ngTableParams', function ($scope, $location, queueService, resourceService, ngTableParams) {
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
                        //$scope.getQueue();
                        data = null;
                        $scope.tableParams.reload();
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

    var data = null;
    var datasplice = null;
    $scope.tableParams = new ngTableParams({
        page: 1,            // show first page
        count: 4          // count per page
    }, {
        total: 0,           // length of data
        //groupBy: '',
        getData: function ($defer, params) {
            if ($scope.authentication.isAuth) {

                if (data == null) {
                    queueService.getQueue()
                        .then(function (response) {
                            $scope.queue = response;
                            for (var i = 0; i < $scope.queue.length; i++) {
                                $scope.queue[i].Status = lookupById($scope.statuses, $scope.queue[i].Status.Id);
                                $scope.queue[i].HowLong = elaspedTime(new Date($scope.queue[i].Order.Date), new Date());
                                $scope.queue[i].color = getElapsedColor($scope.queue[i].HowLong.totalminutes);
                            }
                            params.total(response.length);
                            data = $scope.queue;
                            //$defer.resolve($scope.queue.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                            datasplice = $scope.queue.slice((params.page() - 1) * params.count(), params.page() * params.count());
                            $defer.resolve(datasplice);
                        },
                            function (err) {
                                console.log("queue gotten error " + err);
                                $scope.queue = {};
                            });
                } else {
                    datasplice = data.slice((params.page() - 1) * params.count(), params.page() * params.count());
                    $defer.resolve(datasplice);
                }
            }
        }
    });

    setInterval(function() {
        if (datasplice != null) {
            for (var i = 0; i < datasplice.length; i++) {
                datasplice[i].HowLong = elaspedTime(new Date(datasplice[i].Order.Date), new Date());
                datasplice[i].color = getElapsedColor(datasplice[i].HowLong.seconds);
            }
        }
    }, 1000);
}]);