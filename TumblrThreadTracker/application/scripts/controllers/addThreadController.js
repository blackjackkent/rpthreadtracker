'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('AddThreadController', [
    '$scope', '$routeParams', '$location', 'sessionService', 'contextService', 'blogService', 'threadService', 'pageId', function($scope, $routeParams, $location, sessionService, contextService, blogService, threadService, pageId) {
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
        sessionService.getUser().then(function(user) {
            $scope.userId = user.UserId;
            $scope.user = user;
        });
        blogService.getBlogs().then(function(blogs) {
            $scope.blogs = blogs;
            if (!$scope.currentBlog) {
                $scope.currentBlog = blogs[0].BlogShortname;
            }
        });
    }
]);