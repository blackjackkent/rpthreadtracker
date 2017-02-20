'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('ManageBlogsController',
		[
			'$scope', '$controller', '$location', 'sessionService', 'blogService',
			'threadService', 'pageId', 'notificationService', 'NOTIFICATION_TYPES',
			'BodyClass', '$mdDialog', manageBlogsController
		]);

	/** @this manageBlogsController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function manageBlogsController($scope, $controller, $location, sessionService, blogService, threadService, pageId, notificationService, NOTIFICATION_TYPES, BodyClass, $mdDialog) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		sessionService.loadUser(vm);
		BodyClass.set('');

		initScopeValues();

		function initScopeValues() {
			vm.pageId = pageId;
			vm.shortnameRegex = '[A-z|\\d|\\-]+';
			vm.createBlog = createBlog;
			vm.untrackBlog = untrackBlog;
			vm.toggleHiatus = toggleHiatus;
			blogService.getBlogs(true, true).then(function(blogs) {
				vm.blogs = blogs;
			});
		}

		function success() {
			vm.newBlogForm.$setPristine();
			vm.newBlogShortname = '';
			var type = NOTIFICATION_TYPES.UPDATE_BLOGS_SUCCESS;
			notificationService.show(type);
			blogService.flushBlogs();
			threadService.flushThreads();
			blogService.getBlogs(true, true).then(function(blogs) {
				vm.blogs = blogs;
			});
		}

		function failure() {
			var type = NOTIFICATION_TYPES.UPDATE_BLOGS_FAILURE;
			notificationService.show(type);
		}

		function createBlog() {
			if (!validateNewBlog()) {
				return;
			}
			if (vm.newBlogShortname !== '') {
				blogService.createBlog(vm.newBlogShortname).then(success).catch(failure);
			}
		}

		function validateNewBlog() {
			var shortnameExists = _.some(vm.blogs, function(blog) {
				return blog.BlogShortname === vm.newBlogShortname;
			});
			if (!vm.newBlogForm.$valid || shortnameExists) {
				var emptyShortname = vm.newBlogForm.newBlogShortname.$error.required;
				var invalidShortname = !vm.newBlogForm.newBlogShortname.$error.required &&
					vm.newBlogForm.newBlogShortname.$error.pattern;
				var errorData = {
					'newBlogShortname': vm.newBlogShortname,
					'shortnameExists': shortnameExists,
					'emptyShortname': emptyShortname,
					'invalidShortname': invalidShortname
				};
				var type = NOTIFICATION_TYPES.CREATE_BLOG_VALIDATION_ERROR;
				notificationService.show(type, errorData);
				return false;
			}
			return true;
		}

		function untrackBlog(blog) {
			var message = 'This will untrack all thread(s) ';
			message += 'associated with this blog from your account. Continue?';
			var confirm = $mdDialog.confirm()
				.title('Untrack Blog')
				.textContent(message)
				.ok('Yes')
				.cancel('Cancel');
			$mdDialog.show(confirm).then(function() {
				blogService.untrackBlog(blog.UserBlogId).then(success).catch(failure);
			});
		}

		function toggleHiatus(blog) {
			blog.OnHiatus = !blog.OnHiatus;
			blogService.editBlog(blog).then(success).catch(failure);
		}
	}
}());
