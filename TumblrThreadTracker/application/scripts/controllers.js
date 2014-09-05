'use strict';

/* Controllers */

angular.module('rpThreadTracker.controllers', ['rpThreadTracker.services'])
    .controller('BodyController', [
        '$scope', function($scope) {
            $scope.setBodyClass = function(_bodyClass) {
                $scope.bodyClass = _bodyClass;
            };
        }
    ])
    .controller('MainController', [
        '$scope', '$location', 'threadService', 'contextService', 'blogService', 'newsService', 'sessionService', 'pageId',
        function($scope, $location, threadService, contextService, blogService, newsService, sessionService, pageId) {

            $scope.setBodyClass('');
            sessionService.isLoggedIn().catch(function(isLoggedIn) {
                if (!isLoggedIn) {
                    $location.path('/login');
                }
            });

            function updateThreads(data) {
                $scope.threads = data;
                $scope.myTurnCount = 0;
                $scope.theirTurnCount = 0;
                angular.forEach($scope.threads, function(thread) {
                    $scope.myTurnCount += thread.IsMyTurn ? 1 : 0;
                    $scope.theirTurnCount += thread.IsMyTurn ? 0 : 1;
                });
            }

            $scope.setCurrentBlog = function() {
                contextService.setCurrentBlog($scope.currentBlog);
            };
            $scope.setSortDescending = function() {
                contextService.setSortDescending($scope.sortDescending);
            };
            $scope.setCurrentOrderBy = function() {
                contextService.setCurrentOrderBy($scope.currentOrderBy);
            };
            $scope.pageId = pageId;
            $scope.displayPublicUrl = true;

            threadService.subscribe(updateThreads);
            threadService.getThreads();
            $scope.currentBlog = contextService.getCurrentBlog();
            $scope.sortDescending = contextService.getSortDescending();
            $scope.currentOrderBy = contextService.getCurrentOrderBy();
            sessionService.getUserId().then(function(id) {
                $scope.userId = id;
            });
            blogService.getBlogs().then(function(blogs) {
                $scope.blogs = blogs;
            });

            newsService.getNews().then(function(news) {
                $scope.news = news;
            });

            $scope.untrackThread = function (userThreadId) {
                threadService.flushThreads();
                threadService.untrackThread(userThreadId).then(threadService.getThreads());
            };

            $scope.$on("$destroy", function() {
                threadService.unsubscribe(updateThreads);
            });
        }
    ])
    .controller('PublicController', [
        '$scope', '$routeParams', 'publicThreadService', function ($scope, $routeParams, publicThreadService) {
            $scope.pageId = $routeParams.pageId;
            $scope.userId = $routeParams.userId;
            $scope.currentBlog = $routeParams.currentBlog;
            $scope.currentOrderBy = $routeParams.currentOrderBy;
            $scope.sortDescending = $routeParams.sortDescending;
            $scope.setBodyClass('centered-layout error-page');
            $scope.publicTitleString = buildPublicTitleString();

            function updateThreads(data) {
                console.log(data);
                $scope.threads = data;
            }

            function buildPublicTitleString() {
                var result = "";
                if ($scope.pageId == 'yourturn') {
                    result += "Threads I Owe";
                } else if ($scope.pageId == 'theirturn') {
                    result += "Threads Awaiting Reply";
                } else {
                    result += "All Threads";
                }
                if ($scope.currentBlog != "") {
                    result += " on " + $scope.currentBlog;
                }
                return result;
            }

            publicThreadService.subscribe(updateThreads);
            publicThreadService.getThreads($scope.userId, $scope.currentBlog);

        }
    ])
    .controller('AddThreadController', [
        '$scope', '$routeParams', '$location', 'sessionService', 'contextService', 'blogService', 'threadService', 'pageId', function ($scope, $routeParams, $location, sessionService, contextService, blogService, threadService, pageId) {
            $scope.setBodyClass('');
            sessionService.isLoggedIn().catch(function (isLoggedIn) {
                if (!isLoggedIn) {
                    $location.path('/login');
                }
            });
            function success() {
                $location.path('/threads');
            }
            function failure() {
                $scope.genericError = "There was a problem tracking your thread.";
            }
            $scope.pageId = pageId;
            $scope.currentBlog = contextService.getCurrentBlog();
            $scope.watchedShortname = "";
            $scope.submitThread = function() {
                if (!$scope.currentBlog || !$scope.postId || !$scope.userTitle) {
                    return;
                }
                threadService.flushThreads();
                threadService.addNewThread($scope.currentBlog, $scope.postId, $scope.userTitle, $scope.watchedShortname).then(success, failure);
            };
            sessionService.getUserId().then(function (id) {
                $scope.userId = id;
            });
            blogService.getBlogs().then(function (blogs) {
                $scope.blogs = blogs;
                if (!$scope.currentBlog) {
                    $scope.currentBlog = blogs[0].BlogShortname;
                }
            });
        }
    ])
    .controller('EditThreadController', [
        '$scope', '$routeParams', '$location', 'sessionService', 'contextService', 'blogService', 'threadService', 'pageId', function ($scope, $routeParams, $location, sessionService, contextService, blogService, threadService, pageId) {
            $scope.setBodyClass('');
            sessionService.isLoggedIn().catch(function (isLoggedIn) {
                if (!isLoggedIn) {
                    $location.path('/login');
                }
            });
            function success() {
                $location.path('/threads');
            }
            function failure() {
                $scope.genericError = "There was a problem editing your thread.";
            }
            $scope.pageId = pageId;
            $scope.userThreadId = $routeParams.userThreadId;
            threadService.getStandaloneThread($scope.userThreadId).then(function(thread) {
                console.log(thread);
                $scope.currentBlog = thread.BlogShortname;
                $scope.userTitle = thread.UserTitle;
                $scope.postId = thread.PostId;
                $scope.watchedShortname = thread.WatchedShortname;
                return blogService.getBlogs();
            }).then(function (blogs) {
                $scope.blogs = blogs;
                if (!$scope.currentBlog) {
                    $scope.currentBlog = blogs[0].BlogShortname;
                }
            });
            $scope.submitThread = function() {
                if (!$scope.currentBlog || !$scope.postId || !$scope.userTitle) {
                    return;
                }
                threadService.flushThreads();
                threadService.editThread($scope.userThreadId, $scope.currentBlog, $scope.postId, $scope.userTitle, $scope.watchedShortname).then(success, failure);
            };
            sessionService.getUserId().then(function (id) {
                $scope.userId = id;
            });
        }
    ])
    .controller('ManageBlogsController', [
        '$scope', '$location', 'sessionService', 'blogService', 'pageId', function ($scope, $location, sessionService, blogService, pageId) {
            $scope.setBodyClass('');
            sessionService.isLoggedIn().catch(function (isLoggedIn) {
                if (!isLoggedIn) {
                    $location.path('/login');
                }
            });
            function success() {
                $scope.newBlogForm.$setPristine();
                $scope.newBlogShortname = '';
                blogService.flushBlogs();
                blogService.getBlogs().then(function (blogs) {
                    $scope.blogs = blogs;
                });
            }
            function failure() {
                $scope.genericError = "There was a problem updating your blogs.";
            }
            $scope.createBlog = function() {
                if ($scope.newBlogShortname != '') {
                    blogService.createBlog($scope.newBlogShortname).then(success).catch(failure);
                }
            }
            $scope.untrackBlog = function (userBlogId) {
                blogService.untrackBlog(userBlogId).then(success).catch(failure);
            };
            $scope.pageId = pageId;
            blogService.getBlogs().then(function (blogs) {
                $scope.blogs = blogs;
            });
            sessionService.getUserId().then(function (id) {
                $scope.userId = id;
            });
        }
    ])
    .controller('StaticController', [
        '$scope', 'pageId', function ($scope, pageId) {
            $scope.setBodyClass('');
            $scope.pageId = pageId;
            console.log(pageId);
            $scope.publicView = true;
        }
    ])
    .controller('LoginController', [
        '$scope', '$location', 'sessionService', function($scope, $location, sessionService) {
            var success = function() {
                    $location.path('/');
                },
                fail = function() {
                    $scope.error = "Incorrect username or password.";
                };
            $scope.setBodyClass('signin-page');

            $scope.login = function() {
                sessionService.login($scope.username, $scope.password).then(success, fail);
            };
        }
    ])
    .controller('LogoutController', [
        '$scope', '$location', 'sessionService', function($scope, $location, sessionService) {
            var success = function() {
                $location.path('/');
            };
            sessionService.logout().then(success);
        }
    ]);