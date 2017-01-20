(function() {
	'use strict';
	angular.module('rpthreadtracker')
		.controller('ManageBlogsController',
		[
			'$scope', '$location', 'sessionService', 'blogService', 'threadService', 'pageId', 'TrackerNotification',
			manageBlogsController
		]);

	function manageBlogsController($scope, $location, sessionService, blogService, threadService, pageId, TrackerNotification) {
		$scope.setBodyClass('');
		$scope.pageId = pageId;
		$scope.shortnameRegex = '[A-z|\\d|\\-]+';
		$scope.createBlog = createBlog;
		$scope.untrackBlog = untrackBlog;
		$scope.toggleHiatus = toggleHiatus;
		initView();

		function initView() {
			blogService.getBlogs(true, true)
				.then(function(blogs) {
					$scope.blogs = blogs;
				});
			$scope.emptyBlogShortnameError = new TrackerNotification()
				.withMessage('ERROR: You must enter a blog shortname.')
				.withType('error');
			$scope.invalidBlogShortnameError = new TrackerNotification()
				.withMessage('ERROR: You must enter only the blog shortname, not the full URL.')
				.withType('error');
		}

		function success() {
			$scope.newBlogForm.$setPristine();
			$scope.newBlogShortname = '';
			new TrackerNotification()
				.withMessage("Blogs updated. Click 'Track New Thread' to add a thread for one of the blogs below.")
				.withType('success')
				.show();
			blogService.flushBlogs();
			threadService.flushThreads();
			blogService.getBlogs(true, true)
				.then(function(blogs) {
					$scope.blogs = blogs;
				});
		}

		function failure() {
			new TrackerNotification()
				.withMessage('ERROR: There was a problem updating your blogs.')
				.withType('error')
				.show();
		}

		function createBlog() {
			if (!$scope.newBlogForm.$valid) {
				if ($scope.newBlogForm.newBlogShortname.$error.required) {
					$scope.emptyBlogShortnameError.show();
				}
				if (!$scope.newBlogForm.newBlogShortname.$error.required &&
					$scope.newBlogForm.newBlogShortname.$error.pattern) {
					$scope.invalidBlogShortnameError.show();
				}
				return;
			}
			var shortnameExists = _.findIndex($scope.blogs,
					function(blog) { return blog.BlogShortname == $scope.newBlogShortname; }) !==
				-1;
			if (shortnameExists) {
				$scope.duplicateErrorNotification = new TrackerNotification()
					.withMessage("ERROR: A blog with the shortname '<em>" +
						$scope.newBlogShortname +
						"</em>' is already associated with your account.")
					.withType('error')
					.show();
				return;
			}
			if ($scope.newBlogShortname != '') {
				blogService.createBlog($scope.newBlogShortname).then(success).catch(failure);
			}
		}

		function untrackBlog(userBlogId) {
			blogService.untrackBlog(userBlogId).then(success).catch(failure);
		}

		function toggleHiatus(blog) {
			blog.OnHiatus = !blog.OnHiatus;
			blogService.editBlog(blog).then(success).catch(failure);
		}
	}
})();