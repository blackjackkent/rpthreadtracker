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
        $scope.threadTags = [];
        $scope.submitThread = function() {
            if (!$scope.currentBlog || !$scope.postId || !$scope.userTitle) {
                return;
            }
            threadService.flushThreads();
            threadService.addNewThread($scope.currentBlog, $scope.postId, $scope.userTitle, $scope.watchedShortname, $scope.threadTags).then(success, failure);
        };
        sessionService.getUser().then(function(user) {
            $scope.userId = user.UserId;
            $scope.user = user;
        });
        $scope.handleThreadTagKeypress = function(e) {
            if (e.keyCode == 13) {
                e.originalEvent.preventDefault();
                $scope.addThreadTag($scope.threadTagAddition);
            }
        };
        $scope.handleThreadTagClick = function (e) {
            e.originalEvent.preventDefault();
            $scope.addThreadTag($scope.threadTagAddition);
        };
        $scope.removeThreadTag = function(tag) {
            var index = $scope.threadTags.indexOf(tag);
            if (index > -1) {
                $scope.threadTags.splice(index, 1);
            }
        };
        $scope.addThreadTag = function(tagText) {
            $scope.threadTags.push(tagText);
            $scope.threadTagAddition = "";
        };
        blogService.getBlogs().then(function(blogs) {
            $scope.blogs = blogs;
            if (!$scope.currentBlog) {
                $scope.currentBlog = blogs[0].BlogShortname;
            }
        });
    }
]);