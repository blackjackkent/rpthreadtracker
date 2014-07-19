'use strict';

angular.module('rpThreadTracker.services', [])
    .service('threadService', [
        '$q', '$http', function($q, $http) {
            var subscribers = [],
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

            function getThread(id) {
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
    ])
    .service('blogService', [
        '$q', '$http', function($q, $http) {
            var blogs = [];

            function getBlogs(force) {
                var deferred = $q.defer(),
                    config = {
                        url: '/api/Blog',
                        method: 'GET'
                    },
                    success = function (response) {
                        deferred.resolve(response.data);
                    };
                if (blogs.length > 0 && !force) {
                    deferred.resolve(blogs);
                    return deferred.promise;
                }
                $http(config).then(success);
                return deferred.promise;
            }

            return {
                getBlogs: getBlogs
            };
        }
    ])
    .service('contextService', [
        '$q', '$http', function($q, $http) {
            var currentSort = 'ascending',
                currentBlog = '',
                currentOrderBy = 'LastPostDate';

            function getCurrentSort() {
                return currentSort;
            }

            function getCurrentBlog() {
                return currentBlog;
            }

            function getCurrentOrderBy() {
                return currentOrderBy;
            }

            function setCurrentSort(sort) {
                currentSort = sort;
            }

            function setCurrentBlog(blogShortname) {
                currentBlog = blogShortname;
            }

            function setCurrentOrderBy(orderBy) {
                currentOrderBy = orderBy;
            }

            return {
                getCurrentSort: getCurrentSort,
                getCurrentBlog: getCurrentBlog,
                getCurrentOrderBy: getCurrentOrderBy,
                setCurrentSort: setCurrentSort,
                setCurrentBlog: setCurrentBlog,
                setCurrentOrderBy: setCurrentOrderBy
            };
        }
    ]);