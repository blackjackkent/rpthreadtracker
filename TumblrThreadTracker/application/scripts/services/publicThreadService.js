'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.services.service('publicThreadService', [
    '$q', '$http', function($q, $http) {
        var subscribers = [],
            threads = [];

        function getThreadIds(userId, blogShortname) {
            var deferred = $q.defer(),
                config = {
                    url: '/api/PublicThread?userId=' + userId + "&blogShortname=" + blogShortname,
                    method: 'GET'
                },
                success = function(response) {
                    deferred.resolve(response.data);
                },
                error = function(data) {
                    deferred.reject(data);
                };
            $http(config).then(success).catch(error);
            return deferred.promise;
        }

        function getThreads(userId, blogShortname, force) {
            if (threads.length > 0 && !force) {
                broadcast(threads);
                return;
            }
            threads = [];
            getThreadIds(userId, blogShortname).then(function(ids) {
                angular.forEach(ids, function(value, key) {
                    getThread(value);
                });
            });
        };

        function getThread(id) {
            var deferred = $q.defer(),
                config = {
                    url: '/api/PublicThread/' + id,
                    method: 'GET'
                },
                success = function(response) {
                    threads.push(response.data);
                    broadcast(threads);
                };
            $http(config).then(success);
        };

        function subscribe(callback) {
            subscribers.push(callback);
        }

        function unsubscribe(callback) {
            var index = subscribers.indexOf(callback);
            if (index > -1) {
                subscribers.splice(index, 1);
            }
        }

        function broadcast(data) {
            angular.forEach(subscribers, function(callback, key) {
                callback(data);
            });
        }

        return {
            subscribe: subscribe,
            unsubscribe: unsubscribe,
            getThreads: getThreads
        };
    }
]);