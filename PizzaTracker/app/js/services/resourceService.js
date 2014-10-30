'use strict';
app.factory('resourceService', ['pizzaAppConfig', '$http', '$q', 'localStorageService', function (pizzaAppConfig, $http, $q, localStorageService) {
    var resourceServiceFactory = {};

    resourceServiceFactory.getResources = function (force) {
        var deferred = $q.defer();
        var local = localStorageService.get('resources');
        if (!local || force) {
            $http({
                method: 'GET',
                url: pizzaAppConfig.apiBaseUrl + '/resource'
            }).success(function (response) {
                resourceServiceFactory = response;
                localStorageService.set('resources', response);
                deferred.resolve(response);
            }).error(function (err, status) {
                deferred.reject(err);
            });
        } else {
            resourceServiceFactory = local;
            deferred.resolve(local);
        }
        return deferred.promise;
    };

    return resourceServiceFactory;
}]);