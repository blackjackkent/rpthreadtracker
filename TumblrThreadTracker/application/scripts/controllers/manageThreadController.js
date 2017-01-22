(function() {
	'use strict';
	angular.module('rpthreadtracker')
		.controller('ManageThreadController',
		[
			'$scope', '$routeParams', '$location', 'sessionService', 'contextService', 'blogService', 'threadService',
			'pageId', 'TrackerNotification', '$window', manageThreadController
		]);

	function manageThreadController($scope, $routeParams, $location, sessionService, contextService, blogService, threadService, pageId, TrackerNotification, $window) {
		$scope.setBodyClass('');
		$scope.pageId = pageId;
		$scope.submitThread = submitThread;
		$scope.handleThreadTagKeypress = handleThreadTagKeypress;
		$scope.handleThreadTagClick = handleThreadTagClick;
		$scope.removeThreadTag = removeThreadTag;
		$scope.addThreadTag = addThreadTag;
		initView();

		function initView() {
			$scope.currentBlog = contextService.getCurrentBlog();
			$scope.watchedShortname = '';
			$scope.threadTags = [];
			if (pageId == 'edit-thread') {
				initEditThreadView();
			}
			blogService.getBlogs()
				.then(function(blogs) {
					$scope.blogs = blogs;
					if (!$scope.currentBlog) {
						$scope.currentBlog = blogs[0].BlogShortname;
					}
					if ($routeParams.addFromExtension) {
						initAddFromExtension();
					}
				});
		}

		function initEditThreadView() {
			$scope.userThreadId = $routeParams.userThreadId;
			threadService.getStandaloneThread($scope.userThreadId)
				.then(function(thread) {
					$scope.currentBlog = thread.BlogShortname;
					$scope.userTitle = thread.UserTitle;
					$scope.postId = thread.PostId;
					$scope.watchedShortname = thread.WatchedShortname;
					$scope.isArchived = thread.IsArchived;
					$scope.threadTags = thread.ThreadTags;
					return blogService.getBlogs();
				})
				.then(function(blogs) {
					$scope.blogs = blogs;
					if (!$scope.currentBlog) {
						$scope.currentBlog = blogs[0].BlogShortname;
					}
				});
		}

		function initAddFromExtension() {
			$scope.postId = $routeParams.tumblrPostId;
			var exists = _.find($scope.blogs,
				function(blog) {
					return blog.BlogShortname == $routeParams.tumblrBlogShortname;
				});
			if (!exists) {
				new TrackerNotification()
					.withMessage('WARNING: You are attempting to add a post ID from a blog not associated with this account (' + $routeParams.tumblrBlogShortname + ').' + 'Please use posts from your own blogs, or leave the field blank if you have not posted to the thread yet.')
					.withType('error')
					.show();
			} else {
				$scope.currentBlog = exists.BlogShortname;
			}
		}

		function submitThread() {
			var valid = validateThread();
			if (!valid) {
				return;
			}
			threadService.flushThreads();

			if (pageId == 'edit-thread') {
				threadService.editThread($scope.userThreadId,
						$scope.currentBlog,
						$scope.postId,
						$scope.userTitle,
						$scope.watchedShortname,
						$scope.threadTags,
						$scope.isArchived)
					.then(success, failure);
			} else {
				threadService.addNewThread($scope.currentBlog,
						$scope.postId,
						$scope.userTitle,
						$scope.watchedShortname,
						$scope.threadTags)
					.then(success, failure);
			}
		}

		function validateThread() {
			if (!$scope.currentBlog) {
				new TrackerNotification()
					.withMessage('ERROR: You must select a blog in order to save your thread.')
					.withType('error')
					.show();
				return false;
			}
			if ($scope.newThreadForm.postId.$error.pattern) {
				new TrackerNotification()
					.withMessage('ERROR: Post IDs must contain only numbers.')
					.withType('error')
					.show();
				return false;
			}
			if ($scope.newThreadForm.userTitle.$error.required) {
				new TrackerNotification()
					.withMessage('ERROR: You must enter a thread title for tracking purposes. (This does not have to match a title on the actual Tumblr thread.)')
					.withType('error')
					.show();
				return false;
			}
			return true;
		}

		function handleThreadTagKeypress(e) {
			if (e.keyCode == 13 /** Enter **/ || e.keyCode == 44 /** Comma **/) {
				e.originalEvent.preventDefault();
				if ($scope.threadTagAddition != '' && $scope.threadTagAddition != null) {
					$scope.addThreadTag($scope.threadTagAddition);
				}
			}
		}

		function handleThreadTagClick(e) {
			e.originalEvent.preventDefault();
			if ($scope.threadTagAddition != '' && $scope.threadTagAddition != null) {
				$scope.addThreadTag($scope.threadTagAddition);
			}
		}

		function removeThreadTag(tag) {
			var index = $scope.threadTags.indexOf(tag);
			if (index > -1) {
				$scope.threadTags.splice(index, 1);
			}
		}

		function addThreadTag(tagText) {
			if ($scope.threadTags.indexOf(tagText) == -1) {
				$scope.threadTags.push(tagText);
			}
			$scope.threadTagAddition = '';
		}

		function success() {
			if ($routeParams.addFromExtension) {
				$window.close();
			} else {
				var action = pageId == 'edit-thread' ? 'updated' : 'created';
				new TrackerNotification()
					.withMessage("Thread '<em>" + $scope.userTitle + "</em>' " + action + '.')
					.withType('success')
					.show();
				$location.path('/threads');
			}
		}

		function failure() {
			new TrackerNotification()
				.withMessage('ERROR: There was a problem updating your thread.')
				.withType('error')
				.show();
		}
	}
}());
