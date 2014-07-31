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
                console.log(isLoggedIn);
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

            $scope.$on("$destroy", function() {
                threadService.unsubscribe(updateThreads);
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