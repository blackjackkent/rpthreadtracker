'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('ManageBlogsController', [
    '$scope', '$location', 'sessionService', 'blogService', 'threadService', 'pageId', function($scope, $location, sessionService, blogService, threadService, pageId) {
        $scope.setBodyClass('');
        $scope.shortnameRegex = '[A-z|\\d|\\-]+';

        function success(showSuccessMessage) {
            $scope.newBlogForm.$setPristine();
            $scope.newBlogShortname = '';
            $scope.showSuccessMessage = true;
            blogService.flushBlogs();
            threadService.flushThreads();
            blogService.getBlogs(true, true).then(function(blogs) {
                $scope.blogs = blogs;
            });
        }

        function failure() {
            $scope.genericError = "There was a problem updating your blogs.";
            $scope.showSuccessMessage = false;
        }

        $scope.createBlog = function () {
            if (!$scope.newBlogForm.$valid) {
                return;
            }
            if ($scope.newBlogShortname != '') {
                blogService.createBlog($scope.newBlogShortname).then(success).catch(failure);
            }
        };
        $scope.untrackBlog = function(userBlogId) {
            blogService.untrackBlog(userBlogId).then(success).catch(failure);
        };

        $scope.toggleHiatus = function(blog) {
            blog.OnHiatus = !blog.OnHiatus;
            blogService.editBlog(blog).then(success).catch(failure);
        }
        $scope.pageId = pageId;
        blogService.getBlogs(true, true).then(function(blogs) {
            $scope.blogs = blogs;
        });
        sessionService.getUser().then(function(user) {
            $scope.userId = user.UserId;
            $scope.user = user;
        });
    }
]);