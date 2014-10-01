'use strict';
var rpThreadTracker = rpThreadTracker || {};
rpThreadTracker.controllers.controller('EditBlogController', [
    '$scope', '$routeParams', '$location', 'sessionService', 'contextService', 'blogService', 'threadService', 'pageId', function($scope, $routeParams, $location, sessionService, contextService, blogService, threadService, pageId) {
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
        $scope.submitBlog = function() {
            if (!$scope.newBlogShortname) {
                return;
            }
            blogService.flushBlogs();
            blogService.editBlog($scope.userBlogId, $scope.newBlogShortname).then(success, failure);
        };
        sessionService.getUser().then(function(user) {
            $scope.userId = user.UserId;
            $scope.user = user;
        });
    }
]);