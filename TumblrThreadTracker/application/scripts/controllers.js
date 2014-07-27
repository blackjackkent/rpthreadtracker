'use strict';

/* Controllers */

angular.module('rpThreadTracker.controllers', ['rpThreadTracker.services'])
    .controller('MainController', [
        '$scope', 'threadService', 'contextService', 'blogService', 'newsService', 'pageId', function($scope, threadService, contextService, blogService, newsService, pageId) {

            function updateThreads(data) {
                $scope.threads = data;
                console.log(data);
                $scope.myTurnCount = 0;
                $scope.theirTurnCount = 0;
                angular.forEach($scope.threads, function(thread) {
                    $scope.myTurnCount += thread.IsMyTurn ? 1 : 0;
                    $scope.theirTurnCount += thread.IsMyTurn ? 0 : 1;
                });
            }

            $scope.pageId = pageId;

            threadService.subscribe(updateThreads);
            threadService.getThreads();
            $scope.currentBlog = contextService.getCurrentBlog();
            $scope.sortDescending = contextService.getSortDescending();
            $scope.currentOrderBy = contextService.getCurrentOrderBy();

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
    ]);