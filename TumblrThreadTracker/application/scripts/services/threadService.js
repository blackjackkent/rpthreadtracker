'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.services.service('threadService', [
    '$q', '$http', function($q, $http) {
        var subscribers = [],
            subscribersOnComplete = [],
            threads = [];

        function getThreadIds() {
            var deferred = $q.defer(),
                config = {
                    url: '/api/Thread',
                    method: 'GET'
                },
                success = function (response) {
                    if (response) {
                        deferred.resolve(response.data);
                    } else {
                        deferred.resolve(null);
                    }
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

            broadcast(threads);
            threads = [];
            var queue = [];
            getThreadIds().then(function(ids) {
                angular.forEach(ids, function(value, key) {
                    queue.push(getThread(value));
                });
                $q.all(queue).then(function(results) {
                    broadcastOnComplete();
                });
            });
        };

        function getThread(id) {
            var deferred = $q.defer(),
                config = {
                    url: '/api/Thread/' + id,
                    method: 'GET'
                },
                success = function(response) {
                    threads.push(response.data);
                    broadcast(threads);
                    deferred.resolve(true);
                };
            $http(config).then(success);
            return deferred.promise;
        };

        function getStandaloneThread(id) {
            var deferred = $q.defer(),
                config = {
                    url: '/api/Thread/' + id,
                    method: 'GET'
                },
                success = function(response) {
                    deferred.resolve(response.data);
                },
                error = function(response) {
                    deferred.reject(response.data);
                };
            $http(config).then(success).catch(error);
            return deferred.promise;
        };

        function flushThreads() {
            threads = [];
        }

        function addNewThread(blogShortname, postId, userTitle, watchedShortname) {
            var deferred = $q.defer(),
                config = {
                    url: '/api/Thread',
                    method: "POST",
                    data: {
                        PostId: postId,
                        BlogShortname: blogShortname,
                        UserTitle: userTitle,
                        watchedShortname: watchedShortname
                    }
                },
                success = function(response, status, headers, config) {
                    deferred.resolve(response.data);
                },
                error = function(response, status, headers, config) {
                    deferred.reject(response);
                };
            $http(config).then(success).catch(error);
            return deferred.promise;
        }

        function editThread(userThreadId, blogShortname, postId, userTitle, watchedShortname) {
            var deferred = $q.defer(),
                config = {
                    url: '/api/Thread',
                    method: "PUT",
                    data: {
                        UserThreadId: userThreadId,
                        PostId: postId,
                        BlogShortname: blogShortname,
                        UserTitle: userTitle,
                        watchedShortname: watchedShortname
                    }
                },
                success = function(response, status, headers, config) {
                    deferred.resolve(response.data);
                },
                error = function(response, status, headers, config) {
                    deferred.reject(response);
                };
            $http(config).then(success).catch(error);
            return deferred.promise;
        }

        function untrackThread(userThreadId) {
            var deferred = $q.defer(),
                config = {
                    url: '/api/Thread?userThreadId=' + userThreadId,
                    method: "DELETE"
                },
                success = function(response, status, headers, config) {
                    deferred.resolve(response.data);
                },
                error = function(response, status, headers, config) {
                    deferred.reject(response);
                };
            $http(config).then(success).catch(error);
            return deferred.promise;
        }

        function subscribe(callback) {
            subscribers.push(callback);
        }

        function subscribeOnComplete(callback) {
            subscribersOnComplete.push(callback);
        }

        function unsubscribe(callback) {
            var index = subscribers.indexOf(callback);
            if (index > -1) {
                subscribers.splice(index, 1);
            }
        }

        function unsubscribeOnComplete(callback) {
            var index = subscribersOnComplete.indexOf(callback);
            if (index > -1) {
                subscribers.splice(index, 1);
            }
        }

        function broadcast(data) {
            angular.forEach(subscribers, function(callback, key) {
                callback(data);
            });
        }

        function broadcastOnComplete() {
            angular.forEach(subscribersOnComplete, function(callback, key) {
                callback();
            });
        }

        return {
            subscribe: subscribe,
            unsubscribe: unsubscribe,
            subscribeOnComplete: subscribeOnComplete,
            unsubscribeOnComplete: unsubscribeOnComplete,
            getThreads: getThreads,
            getStandaloneThread: getStandaloneThread,
            addNewThread: addNewThread,
            editThread: editThread,
            untrackThread: untrackThread,
            flushThreads: flushThreads
        };
    }
]);