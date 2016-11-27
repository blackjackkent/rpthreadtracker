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
        blogService.getStandaloneBlog($routeParams.userBlogId).then(function(blog) {
            $scope.blogToEdit = blog;
            $scope.currentBlogShortname = angular.copy($scope.blogToEdit.BlogShortname);
        });
        $scope.submitBlog = function() {
            if (!$scope.blogToEdit.BlogShortname) {
                return;
            }
            blogService.flushBlogs();
            blogService.editBlog($scope.blogToEdit).then(success, failure);
        };
        sessionService.getUser().then(function(user) {
            $scope.user = user;
        });
    }
]);