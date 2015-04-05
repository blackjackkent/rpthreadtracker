'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('EditThreadController', [
    '$scope', '$routeParams', '$location', 'sessionService', 'contextService', 'blogService', 'threadService', 'pageId', function($scope, $routeParams, $location, sessionService, contextService, blogService, threadService, pageId) {
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
            $scope.isArchived = thread.IsArchived;
            return blogService.getBlogs();
        }).then(function(blogs) {
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
            threadService.editThread($scope.userThreadId, $scope.currentBlog, $scope.postId, $scope.userTitle, $scope.watchedShortname, $scope.isArchived).then(success, failure);
        };
        sessionService.getUser().then(function(user) {
            $scope.userId = user.UserId;
            $scope.user = user;
        });
    }
]);