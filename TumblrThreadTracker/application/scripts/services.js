'use strict';

angular.module('rpThreadTracker.services', [])
    .service('threadService', [
        '$q', '$http', function($q, $http) {
            var _subscribers = [],
                threads = [];

            function getThreadIds() {
                var deferred = $q.defer(),
                    config = {
                        url: '/api/Thread',
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

            function getThreads(force) {
                if (threads.length > 0 && !force) {
                    broadcast(threads);
                    return;
                }
                threads = [];
                getThreadIds().then(function(ids) {
                    angular.forEach(ids, function(value, key) {
                        getThread(value);
                    });
                });
            };

            var getThread = function(id) {
                var deferred = $q.defer(),
                    config = {
                        url: '/api/Thread/' + id,
                        method: 'GET'
                    },
                    success = function(response) {
                        threads.push(response.data);
                        broadcast(threads);
                    };
                $http(config).then(success);
            };


            function subscribe(callback) {
                _subscribers.push(callback);
            }

            function unsubscribe(callback) {
                var index = _subscribers.indexOf(callback);
                if (index > -1) {
                    _subscribers.splice(index, 1);
                }
            }

            function broadcast(data) {
                angular.forEach(_subscribers, function (callback, key) {
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