'use strict';

/* Controllers */

angular.module('rpThreadTracker.controllers', ['rpThreadTracker.services'])
    .controller('MainController', [
        '$scope', 'threadService', 'contextService', 'blogService', function($scope, threadService, contextService, blogService) {

            function updateThreads(data) {
                $scope.threads = data;
                $scope.myTurnCount = 0;
                $scope.theirTurnCount = 0;
                angular.forEach($scope.threads, function(thread) {
                    $scope.myTurnCount += thread.IsMyTurn ? 1 : 0;
                    $scope.theirTurnCount += thread.IsMyTurn ? 0 : 1;
                });
            }

            threadService.subscribe(updateThreads);
            threadService.getThreads();
            $scope.currentBlog = contextService.getCurrentBlog();
            $scope.currentSort = contextService.getCurrentSort();
            $scope.currentOrderBy = contextService.getCurrentOrderBy();

            blogService.getBlogs().then(function(blogs) {
                $scope.blogs = blogs;
            });

            $scope.$on("$destroy", function() {
                threadService.unsubscribe(updateThreads);
            });
        }
    ]);