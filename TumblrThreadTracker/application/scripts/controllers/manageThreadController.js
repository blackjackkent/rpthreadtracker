'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('ManageThreadController',
		[
			'$scope', '$controller', '$routeParams', '$location', 'sessionService',
			'contextService', 'blogService', 'threadService', 'pageId',
			'TrackerNotification', '$window', 'BodyClass', manageThreadController
		]);

	/** @this manageThreadController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function manageThreadController($scope, $controller, $routeParams, $location, sessionService, contextService, blogService, threadService, pageId, TrackerNotification, $window, BodyClass) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		sessionService.loadUser(vm);
		BodyClass.set('');

		vm.isEditPage = pageId === 'edit-thread';
		vm.isExtensionPage = $routeParams.addFromExtension;
		blogService.getBlogs().then(function(blogs) {
			vm.blogs = blogs;
			initScopeValues();
			initScopeFunctions();
			if (vm.isEditPage) {
				initEditThreadView();
			}
			if (vm.isExtensionPage) {
				initAddFromExtension();
			}
		});

		function initScopeValues() {
			vm.pageId = pageId;
			vm.thread = {};
			vm.thread.ThreadTags = [];
			var currentBlog = contextService.getCurrentBlog();
			if (!currentBlog) {
				currentBlog = _.head(vm.blogs);
			}
			if (currentBlog && currentBlog.UserBlogId) {
				vm.thread.UserBlogId = currentBlog.UserBlogId;
			}
		}

		function initScopeFunctions() {
			vm.submitThread = submitThread;
			vm.handleThreadTagKeypress = handleThreadTagKeypress;
			vm.handleThreadTagClick = handleThreadTagClick;
			vm.removeThreadTag = removeThreadTag;
		}

		function initEditThreadView() {
			var userThreadId = $routeParams.userThreadId;
			threadService.getStandaloneThread(userThreadId).then(function(thread) {
				vm.thread = thread;
				return blogService.getBlogs();
			}).then(function(blogs) {
				vm.blogs = blogs;
				if (!vm.currentBlog) {
					vm.currentBlog = blogs[0].BlogShortname;
				}
			});
		}

		function initAddFromExtension() {
			vm.thread.PostId = $routeParams.tumblrPostId;
			var exists = _.find(vm.blogs, function(blog) {
				return blog.BlogShortname === $routeParams.tumblrBlogShortname;
			});
			if (exists) {
				vm.thread.UserBlogId = exists.UserBlogId;
			} else {
				var message = 'WARNING: You are attempting to add a post ID from a blog ';
				message += 'not associated with this account (';
				message += $routeParams.tumblrBlogShortname;
				message += '). Please use posts from your own blogs, or leave the';
				message += 'field blank if you have not posted to the thread yet.';
				new TrackerNotification()
					.withMessage(message)
					.withType('error')
					.show();
			}
		}

		function submitThread() {
			var valid = validateThread();
			if (!valid) {
				return;
			}
			threadService.flushThreads();
			if (vm.isEditPage) {
				threadService.editThread(vm.thread)
					.then(success, failure);
			} else {
				threadService.addNewThread(vm.thread)
					.then(success, failure);
			}
		}

		function handleThreadTagKeypress(e) {
			// 13: Enter, 44: Comma
			if (e.keyCode === 13 || e.keyCode === 44) {
				e.originalEvent.preventDefault();
				if (vm.threadTagAddition) {
					addThreadTag(vm.threadTagAddition);
				}
			}
		}

		function handleThreadTagClick(e) {
			e.originalEvent.preventDefault();
			if (vm.threadTagAddition) {
				addThreadTag(vm.threadTagAddition);
			}
		}

		function removeThreadTag(tag) {
			var index = vm.thread.ThreadTags.indexOf(tag);
			if (index > -1) {
				vm.thread.ThreadTags.splice(index, 1);
			}
		}

		function addThreadTag(tagText) {
			if (vm.thread.ThreadTags.indexOf(tagText) === -1) {
				vm.thread.ThreadTags.push(tagText);
			}
			vm.threadTagAddition = '';
		}

		function success() {
			if ($routeParams.addFromExtension) {
				$window.close();
			} else {
				var action = vm.isEditPage ? 'updated' : 'created';
				new TrackerNotification()
					.withMessage("Thread '<em>" + vm.thread.UserTitle + "</em>' " + action + '.')
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

		// eslint-disable-next-line max-statements
		function validateThread() {
			if (vm.newThreadForm.postId.$error.pattern) {
				new TrackerNotification()
					.withMessage('ERROR: Post IDs must contain only numbers.')
					.withType('error')
					.show();
				return false;
			}
			if (vm.newThreadForm.userTitle.$error.required) {
				var message = 'ERROR: You must enter a thread title for tracking purposes.';
				message += ' (This does not have to match a title on the actual Tumblr thread.)';
				new TrackerNotification()
					.withMessage(message)
					.withType('error')
					.show();
				return false;
			}
			return true;
		}
	}
}());
