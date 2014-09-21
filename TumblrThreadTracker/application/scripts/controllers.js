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
            $scope.dashboardFilter = 'yourturn';

            threadService.subscribe(updateThreads);
            threadService.getThreads();
            $scope.currentBlog = contextService.getCurrentBlog();
            $scope.sortDescending = contextService.getSortDescending();
            $scope.currentOrderBy = contextService.getCurrentOrderBy();
            sessionService.getUser().then(function(user) {
                $scope.userId = user.UserId;
                $scope.user = user;
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
            $scope.refreshThreads = function() { threadService.getThreads(true); };

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
            sessionService.getUser().then(function (user) {
                $scope.userId = user.UserId;
                $scope.user = user;
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
            function success() {
                $location.path('/threads');
            }
            function failure() {
                $scope.genericError = "There was a problem editing your thread.";
            }
            $scope.pageId = pageId;
            $scope.userThreadId = $routeParams.userThreadId;
            threadService.getStandaloneThread($scope.userThreadId).then(function(thread) {
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
            sessionService.getUser().then(function (user) {
                $scope.userId = user.UserId;
                $scope.user = user;
            });
        }
    ])
    .controller('ManageBlogsController', [
        '$scope', '$location', 'sessionService', 'blogService', 'threadService', 'pageId', function ($scope, $location, sessionService, blogService, threadService, pageId) {
            $scope.setBodyClass('');
            function success() {
                $scope.newBlogForm.$setPristine();
                $scope.newBlogShortname = '';
                $scope.showSuccessMessage = true;
                blogService.flushBlogs();
                threadService.flushThreads();
                blogService.getBlogs().then(function (blogs) {
                    $scope.blogs = blogs;
                });
            }
            function failure() {
                $scope.genericError = "There was a problem updating your blogs.";
                $scope.showSuccessMessage = false;
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
            sessionService.getUser().then(function (user) {
                $scope.userId = user.UserId;
                $scope.user = user;
            });
        }
    ])
    .controller('EditBlogController', [
        '$scope', '$routeParams', '$location', 'sessionService', 'contextService', 'blogService', 'threadService', 'pageId', function ($scope, $routeParams, $location, sessionService, contextService, blogService, threadService, pageId) {
            $scope.setBodyClass('');
            function success() {
                $location.path('/manage-blogs');
            }
            function failure() {
                $scope.genericError = "There was a problem editing your blog.";
            }
            $scope.pageId = pageId;
            $scope.userBlogId = $routeParams.userBlogId;
            blogService.getStandaloneBlog($scope.userBlogId).then(function(blog) {
                $scope.blogToEdit = blog;
            });
            $scope.submitBlog = function () {
                if (!$scope.newBlogShortname) {
                    return;
                }
                blogService.flushBlogs();
                blogService.editBlog($scope.userBlogId, $scope.newBlogShortname).then(success, failure);
            };
            sessionService.getUser().then(function (user) {
                $scope.userId = user.UserId;
                $scope.user = user;
            });
        }
    ])
    .controller('ManageAccountController', [
        '$scope', '$location', 'sessionService', 'blogService', 'threadService', 'pageId', function ($scope, $location, sessionService, blogService, threadService, pageId) {
            $scope.setBodyClass('');
            function success() {
                $scope.newBlogForm.$setPristine();
                $scope.newBlogShortname = '';
                $scope.showSuccessMessage = true;
                blogService.flushBlogs();
                threadService.flushThreads();
                blogService.getBlogs().then(function (blogs) {
                    $scope.blogs = blogs;
                });
            }
            function failure() {
                $scope.genericError = "There was a problem updating your blogs.";
                $scope.showSuccessMessage = false;
            }
            $scope.createBlog = function () {
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
            sessionService.getUser().then(function (user) {
                $scope.userId = user.UserId;
                $scope.user = user;
            });
        }
    ])
    .controller('StaticController', [
        '$scope', 'sessionService', 'pageId', function ($scope, sessionService, pageId) {
            $scope.setBodyClass('');
            $scope.pageId = pageId;
            $scope.publicView = true;
            sessionService.isLoggedIn().then(function (isLoggedIn) {
                if (isLoggedIn) {
                    sessionService.getUser().then(function(user) {
                        $scope.userId = user.UserId;
                        $scope.user = user;
                    });
                }
            });
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
            sessionService.isLoggedIn().then(function (isLoggedIn) {
                if (isLoggedIn) {
                    $location.path('/');
                }
            });
            $scope.setBodyClass('signin-page');

            $scope.login = function() {
                sessionService.login($scope.username, $scope.password).then(success, fail);
            };
        }
    ])
    .controller('ForgotPasswordController', [
        '$scope', '$location', 'sessionService', function ($scope, $location, sessionService) {
            var success = function () {
                $scope.success = "Success. Check your email box for a temporary password."
            },
                fail = function () {
                    $scope.error = "Unknown error. Please try again later.";
                };
            $scope.setBodyClass('signin-page');

            $scope.submitForgotPassword = function () {
                $scope.error = "";
                $scope.success = "";
                sessionService.submitForgotPassword($scope.username).then(success, fail);
            };
        }
    ])
    .controller('RegisterController', [
        '$scope', '$location', 'sessionService', function ($scope, $location, sessionService) {
            var success = function () {
                $location.path('/');
            },
                fail = function () {
                    $scope.error = "Error registering account. Please try again later.";
                };
            $scope.setBodyClass('signin-page');

            $scope.register = function () {
                $scope.error = "";
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