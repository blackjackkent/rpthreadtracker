'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('ManageThreadController', [
    '$scope', '$routeParams', '$location', 'sessionService', 'contextService', 'blogService', 'threadService', 'pageId', function($scope, $routeParams, $location, sessionService, contextService, blogService, threadService, pageId) {
        $scope.setBodyClass('');

        $scope.pageId = pageId;
        if (pageId == 'edit-thread') {
            $scope.userThreadId = $routeParams.userThreadId;
            threadService.getStandaloneThread($scope.userThreadId).then(function(thread) {
                $scope.currentBlog = thread.BlogShortname;
                $scope.userTitle = thread.UserTitle;
                $scope.postId = thread.PostId;
                $scope.watchedShortname = thread.WatchedShortname;
                $scope.isArchived = thread.IsArchived;
                $scope.threadTags = thread.ThreadTags;
                return blogService.getBlogs();
            }).then(function(blogs) {
                $scope.blogs = blogs;
                if (!$scope.currentBlog) {
                    $scope.currentBlog = blogs[0].BlogShortname;
                }
            });
        } else {
            $scope.currentBlog = contextService.getCurrentBlog();
            $scope.watchedShortname = "";
            $scope.threadTags = [];
        }
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

        // ********* functions **********
        $scope.submitThread = function() {
            if (!$scope.currentBlog || !$scope.userTitle) {
                $scope.genericError = "You must select a blog and enter a title in order to save your thread.";
                return;
            }
            threadService.flushThreads();

            if (pageId == 'edit-thread') {
                threadService.editThread($scope.userThreadId, $scope.currentBlog, $scope.postId, $scope.userTitle, $scope.watchedShortname, $scope.threadTags, $scope.isArchived).then(success, failure);
            } else {
                threadService.addNewThread($scope.currentBlog, $scope.postId, $scope.userTitle, $scope.watchedShortname, $scope.threadTags).then(success, failure);
            }
        };
        $scope.handleThreadTagKeypress = function (e) {
            if (e.keyCode == 13 /** enter **/ || e.keyCode == 44 /** comma **/) {
                e.originalEvent.preventDefault();
                if ($scope.threadTagAddition != "" && $scope.threadTagAddition != null) {
                    $scope.addThreadTag($scope.threadTagAddition);
                }
            }
        };
        $scope.handleThreadTagClick = function (e) {
            e.originalEvent.preventDefault();
            if ($scope.threadTagAddition != "" && $scope.threadTagAddition != null) {
                $scope.addThreadTag($scope.threadTagAddition);
            }
        };
        $scope.removeThreadTag = function(tag) {
            var index = $scope.threadTags.indexOf(tag);
            if (index > -1) {
                $scope.threadTags.splice(index, 1);
            }
        };
        $scope.addThreadTag = function (tagText) {
            if ($scope.threadTags.indexOf(tagText) == -1) {
                $scope.threadTags.push(tagText);
            }
            $scope.threadTagAddition = "";
        };

        function success() {
            $location.path('/threads');
        }

        function failure() {
            $scope.genericError = "There was a problem updating your thread.";
        }

    }
]);