'use strict';
app.factory('resourceService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var resourceServiceFactory = {};

    var _getResources = function (force) {
        var deferred = $q.defer();
        var local = localStorageService.get('resources');
        if (!local || force) {
            console.log("going to get resources");
            $http({
                method: 'GET',
                url: '/api/resource'
            }).success(function (response) {
                resourceServiceFactory = response;
                localStorageService.set('resources', response);
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
        } else {
            console.log("local resources");
            resourceServiceFactory = local;
            deferred.resolve(local);
        }
        return deferred.promise;
    };
    resourceServiceFactory.getResources = _getResources;

    return resourceServiceFactory;
}]);