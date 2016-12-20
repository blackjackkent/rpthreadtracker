'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('ManageThreadController', [
    '$scope', '$routeParams', '$location', 'sessionService', 'contextService', 'blogService', 'threadService', 'pageId', 'Notification', '$window',
        function ($scope, $routeParams, $location, sessionService, contextService, blogService, threadService, pageId, Notification, $window) {
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
        if ($routeParams.addFromExtension) {
            $scope.postId = $routeParams.tumblrPostId;
        }
        blogService.getBlogs().then(function (blogs) {
            $scope.blogs = blogs;
            if (!$scope.currentBlog) {
                $scope.currentBlog = blogs[0].BlogShortname;
            }
            if ($routeParams.addFromExtension) {
                var exists = _.find(blogs,
                    function(blog) {
                        return blog.BlogShortname == $routeParams.tumblrBlogShortname;
                    });
                console.log(blogs);
                console.log(exists);
                if (!exists) {
                    displayTumblrShortnameWarning($routeParams.tumblrBlogShortname);
                } else {
                    $scope.currentBlog = exists.BlogShortname;
                }
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
            if ($routeParams.addFromExtension) {
                $window.close();
            } else {
                $location.path('/threads');
            }
        }

        function failure() {
            $scope.genericError = "There was a problem updating your thread.";
        }

        function displayTumblrShortnameWarning(shortname) {
            Notification.error('WARNING: You are attempting to add a post ID from a blog not associated with this account (' + shortname + ').'
                + 'Please use posts from your own blogs, or leave the field blank if you have not posted to the thread yet.');
        }
    }
]);