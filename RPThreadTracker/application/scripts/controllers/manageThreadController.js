'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('ManageThreadController',
		[
			'$scope', '$controller', '$routeParams', '$location', 'sessionService',
			'contextService', 'blogService', 'threadService', 'pageId',
			'notificationService', 'NOTIFICATION_TYPES', '$window',
			'BodyClass', manageThreadController
		]);

	/** @this manageThreadController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function manageThreadController($scope, $controller, $routeParams, $location, sessionService, contextService, blogService, threadService, pageId, notificationService, NOTIFICATION_TYPES, $window, BodyClass) {
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
			});
		}

		function initAddFromExtension() {
			vm.thread.PostId = $routeParams.tumblrPostId;
			var routeShortname = $routeParams.tumblrBlogShortname.toLowerCase();
			var exists = _.find(vm.blogs, function(blog) {
				return blog.BlogShortname.toLowerCase() === routeShortname;
			});
			if (exists) {
				vm.thread.UserBlogId = exists.UserBlogId;
			} else {
				var type = NOTIFICATION_TYPES.CREATE_THREAD_EXT_BLOG_DNE;
				var extraData = {'tumblrBlogShortname': $routeParams.tumblrBlogShortname};
				notificationService.show(type, extraData);
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
				var type = NOTIFICATION_TYPES.CREATE_THREAD_SUCCESS;
				var extraData = {
					'action': action,
					'userTitle': vm.thread.UserTitle
				};
				notificationService.show(type, extraData);
				$location.path('/threads');
			}
		}

		function failure() {
			var type = NOTIFICATION_TYPES.CREATE_THREAD_FAILURE;
			notificationService.show(type);
		}

		function validateThread() {
			var extraData = {};
			if (vm.newThreadForm.postId.$error.pattern) {
				extraData.errorPattern = true;
			}
			if (vm.newThreadForm.userTitle.$error.required) {
				extraData.errorRequired = true;
			}
			if (extraData.errorPattern || extraData.errorRequired) {
				var type = NOTIFICATION_TYPES.CREATE_THREAD_VALIDATION_ERROR;
				notificationService.show(type, extraData);
				return false;
			}
			return true;
		}
	}
}());
