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
    .service('newsService', [
        '$q', '$http', function ($q, $http) {
            var news = [];

            function getNews(force) {
                var deferred = $q.defer(),
                    config = {
                        url: '/api/News',
                        method: 'GET'
                    },
                    success = function (response) {
                        deferred.resolve(response.data);
                    };
                if (news.length > 0 && !force) {
                    deferred.resolve(news);
                    return deferred.promise;
                }
                $http(config).then(success);
                return deferred.promise;
            }

            return {
                getNews: getNews
            };
        }
    ])
    .service('contextService', [
        '$q', '$http', function($q, $http) {
            var sortDescending = false,
                currentBlog = '',
                currentOrderBy = 'LastPostDate';

            function getSortDescending() {
                return sortDescending;
            }

            function getCurrentBlog() {
                return currentBlog;
            }

            function getCurrentOrderBy() {
                return currentOrderBy;
            }

            function setSortDescending(sort) {
                sortDescending = sort;
            }

            function setCurrentBlog(blogShortname) {
                currentBlog = blogShortname;
            }

            function setCurrentOrderBy(orderBy) {
                currentOrderBy = orderBy;
            }

            return {
                getSortDescending: getSortDescending,
                getCurrentBlog: getCurrentBlog,
                getCurrentOrderBy: getCurrentOrderBy,
                setSortDescending: setSortDescending,
                setCurrentBlog: setCurrentBlog,
                setCurrentOrderBy: setCurrentOrderBy
            };
        }
    ]);