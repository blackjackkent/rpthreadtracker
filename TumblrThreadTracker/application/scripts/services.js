'use strict';

angular.module('rpThreadTracker.services', [])
    .service('threadService', [
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

                broadcast(threads);
                threads = [];
                var queue = [];
                getThreadIds().then(function(ids) {
                    angular.forEach(ids, function(value, key) {
                        queue.push(getThread(value));
                    });
                    $q.all(queue).then(function (results) {
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
    ])
    .service('publicThreadService', [
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
                    success = function(response) {
                        deferred.resolve(response.data);
                    };
                if (blogs.length > 0 && !force) {
                    deferred.resolve(blogs);
                    return deferred.promise;
                }
                $http(config).then(success);
                return deferred.promise;
            }

            function getStandaloneBlog(id) {
                var deferred = $q.defer(),
                    config = {
                        url: '/api/Blog/' + id,
                        method: 'GET'
                    },
                    success = function(response) {
                        console.log(id);
                        deferred.resolve(response.data);
                    },
                    error = function(response) {
                        deferred.reject(response.data);
                    };
                $http(config).then(success).catch(error);
                return deferred.promise;
            };

            function flushBlogs() {
                blogs = [];
            }

            function createBlog(blogShortname) {
                var deferred = $q.defer(),
                    config = {
                        url: '/api/Blog',
                        method: "POST",
                        data: {
                            BlogShortname: blogShortname
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

            function editBlog(userBlogId, newBlogShortname) {
                var deferred = $q.defer(),
                    config = {
                        url: '/api/Blog',
                        method: "PUT",
                        data: {
                            UserBlogId: userBlogId,
                            BlogShortname: newBlogShortname
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

            function untrackBlog(userBlogId) {
                var deferred = $q.defer(),
                    config = {
                        url: '/api/Blog?userBlogId=' + userBlogId,
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

            return {
                getBlogs: getBlogs,
                getStandaloneBlog: getStandaloneBlog,
                flushBlogs: flushBlogs,
                createBlog: createBlog,
                editBlog: editBlog,
                untrackBlog: untrackBlog
            };
        }
    ])
    .service('newsService', [
        '$q', '$http', function($q, $http) {
            var news = [];

            function getNews(force) {
                var deferred = $q.defer(),
                    config = {
                        url: '/api/News',
                        method: 'GET'
                    },
                    success = function(response) {
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

            function getPublicUrl(pageId, userId) {
                return "http://www.rpthreadtracker.com/public/" + pageId + "?userId=" + userId + "&currentBlog=" + currentBlog + "&sortDescending=" + sortDescending + "&currentOrderBy=" + currentOrderBy;
            }

            return {
                getSortDescending: getSortDescending,
                getCurrentBlog: getCurrentBlog,
                getCurrentOrderBy: getCurrentOrderBy,
                setSortDescending: setSortDescending,
                setCurrentBlog: setCurrentBlog,
                setCurrentOrderBy: setCurrentOrderBy,
                getPublicUrl: getPublicUrl
            };
        }
    ])
    .service('sessionService', [
        '$q', '$http', function($q, $http) {
            var user = null;

            function getUser(force) {
                var deferred = $q.defer(),
                    config = {
                        url: '/api/User',
                        method: 'GET'
                    },
                    success = function (response) {
                        user = response.data;
                        deferred.resolve(response.data);
                    };
                if (user != null && !force) {
                    deferred.resolve(user);
                    return deferred.promise;
                }
                $http(config).then(success);
                return deferred.promise;
            }

            function isLoggedIn() {
                var deferred = $q.defer(),
                    config = {
                        method: 'GET',
                        url: '/api/User'
                    },
                    success = function(response, status, headers, config) {
                        deferred.resolve(response);
                    },
                    error = function(data) {
                        deferred.reject(false);
                    };
                $http(config).then(success).catch(error);
                return deferred.promise;
            }

            function login(username, password) {
                var deferred = $q.defer(),
                    config = {
                        method: 'POST',
                        url: '/api/Session',
                        data: { UserName: username, Password: password }
                    },
                    success = function(response, status, headers, config) {
                        deferred.resolve(response.data);
                    },
                    error = function(data) {
                        deferred.reject(data);
                    };
                $http(config).then(success).catch(error);
                return deferred.promise;
            }

            function logout() {
                var deferred = $q.defer(),
                    config = {
                        method: 'DELETE',
                        url: '/api/Session'
                    },
                    success = function(response, status, headers, config) {
                        deferred.resolve(response.data);
                        user = null;
                    },
                    error = function(data) {
                        deferred.reject(data);
                    };
                $http(config).then(success).catch(error);
                return deferred.promise;
            }

            function submitForgotPassword(username) {
                var deferred = $q.defer(),
                    config = {
                        method: 'POST',
                        url: '/api/ForgotPassword?username=' + username
                    },
                    success = function(response, status, headers, config) {
                        deferred.resolve(response.data);
                    },
                    error = function(data) {
                        deferred.reject(data);
                    };
                $http(config).then(success).catch(error);
                return deferred.promise;
            }

            function register(username, email, password, confirmPassword) {
                var deferred = $q.defer(),
                     config = {
                         url: '/api/Account',
                         method: "POST",
                         data: {
                             Username: username,
                             Email: email,
                             Password: password,
                             ConfirmPassword: confirmPassword
                         }
                     },
                     success = function (response, status, headers, config) {
                         deferred.resolve(response.data);
                     },
                     error = function (response, status, headers, config) {
                         deferred.reject(response);
                     };
                $http(config).then(success).catch(error);
                return deferred.promise;
            }

            function changePassword(oldPassword, newPassword, confirmNewPassword) {
                var deferred = $q.defer(),
                     config = {
                         url: '/api/ChangePassword',
                         method: "POST",
                         data: {
                             OldPassword: oldPassword,
                             NewPassword: newPassword,
                             ConfirmNewPassword: confirmNewPassword
                         }
                     },
                     success = function (response, status, headers, config) {
                         deferred.resolve(response.data);
                     },
                     error = function (response, status, headers, config) {
                         deferred.reject(response);
                     };
                $http(config).then(success).catch(error);
                return deferred.promise;
            }

            return {
                isLoggedIn: isLoggedIn,
                login: login,
                logout: logout,
                submitForgotPassword: submitForgotPassword,
                changePassword: changePassword,
                register: register,
                getUser: getUser
            };
        }
    ]);