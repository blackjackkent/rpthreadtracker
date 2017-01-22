(function() {
	'use strict';
	angular.module('rpthreadtracker')
		.controller('EditBlogController',
		[
			'$scope', '$routeParams', '$location', 'sessionService', 'contextService', 'blogService', 'threadService',
			'TrackerNotification', 'pageId',
			editBlogController
		]);

	function editBlogController($scope, $routeParams, $location, sessionService, contextService, blogService, threadService, TrackerNotification, pageId) {
		$scope.setBodyClass('');
		$scope.pageId = pageId;
		$scope.submitBlog = submitBlog;
		initView($routeParams.userBlogId);

		function submitBlog() {
			if (!$scope.blogToEdit.BlogShortname) {
				$scope.missingValueNotification.show();
				return;
			}
			var shortnameExists = _.findIndex($scope.allBlogs,
					function(blog) {
						return blog.BlogShortname == $scope.blogToEdit.BlogShortname;
					}) !==
				-1;
			if (shortnameExists) {
				$scope.duplicateErrorNotification.show();
				return;
			}
			blogService.flushBlogs();
			blogService.editBlog($scope.blogToEdit).then(success, failure);
		}

		function initView(userBlogId) {
			blogService.getBlogs(true, true)
				.then(function(blogs) {
					$scope.allBlogs = blogs;
					$scope.blogToEdit = angular.copy(_.find(blogs,
						function(blog) {
							return blog.UserBlogId == userBlogId;
						}));
					$scope.currentBlogShortname = angular.copy($scope.blogToEdit.BlogShortname);
				});
			$scope.duplicateErrorNotification = new TrackerNotification()
				.withMessage('ERROR: A blog with this shortname is already associated with your account.')
				.withType('error');
			$scope.generalErrorMessage = new TrackerNotification()
				.withMessage('ERROR: There was a problem editing your blog.')
				.withType('error');
			$scope.missingValueNotification = new TrackerNotification()
				.withMessage('ERROR: You must enter a blog shortname.')
				.withType('error');
		}

		function success() {
			new TrackerNotification()
				.withMessage($scope.currentBlogShortname + ' renamed to ' + $scope.blogToEdit.BlogShortname + '.')
				.withType('success')
				.show();
			$location.path('/manage-blogs');
		}

		function failure() {
			$scope.generalErrorMessage.show();
		}
	}
}());
