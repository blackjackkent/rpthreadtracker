'use strict';

/* Services */


// Demonstrate how to register services
// In this case it is a simple value service.
angular.module('rpThreadTracker.services', [])
    .service('threadService', ['$q', '$http', function($q, $http) {
        var threadIds = [];

        function getThreadIds(force) {
            var deferred = $q.defer(),
                config = {
                    url: '/api/Thread',
                    method: 'GET'
                },
                success = function(response) {
                    threadIds = response.data;
                    deferred.resolve(response.data);
                },
                error = function(data) {
                    deferred.reject(data);
                };
            if (!force && threadIds.length > 0) {
                deferred.resolve(threadIds);
            }
            $http(config).then(success).catch(error);
            return deferred.promise;
        }

    return {
        getThreadIds: getThreadIds
    };
}])
