'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('ManageBlogsController',
		[
			'$scope', '$controller', '$location', 'sessionService', 'blogService',
			'threadService', 'pageId', 'TrackerNotification', 'BodyClass', '$mdDialog',
			manageBlogsController
		]);

	/** @this manageBlogsController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function manageBlogsController($scope, $controller, $location, sessionService, blogService, threadService, pageId, TrackerNotification, BodyClass, $mdDialog) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		sessionService.loadUser(vm);
		BodyClass.set('');

		initScopeValues();
		initView();

		function initScopeValues() {
			vm.pageId = pageId;
			vm.shortnameRegex = '[A-z|\\d|\\-]+';
			vm.createBlog = createBlog;
			vm.untrackBlog = untrackBlog;
			vm.toggleHiatus = toggleHiatus;
		}

		function initView() {
			blogService.getBlogs(true, true).then(function(blogs) {
				vm.blogs = blogs;
			});
			vm.emptyBlogShortnameError = new TrackerNotification()
				.withMessage('ERROR: You must enter a blog shortname.')
				.withType('error');
			vm.invalidBlogShortnameError = new TrackerNotification()
				.withMessage('ERROR: You must enter only the blog shortname, not the full URL.')
				.withType('error');
			var successMessage = "Blogs updated. Click 'Track New Thread' ";
			successMessage += 'to add a thread for one of the blogs below.';
			vm.successMessage = new TrackerNotification()
				.withMessage(successMessage)
				.withType('success');
			vm.failureMessage = new TrackerNotification()
				.withMessage('ERROR: There was a problem updating your blogs.')
				.withType('error');
			vm.duplicateErrorNotification = new TrackerNotification()
				.withMessage("ERROR: A blog with the shortname '<em>" +
					vm.newBlogShortname +
					"</em>' is already associated with your account.")
				.withType('error');
		}

		function success() {
			vm.newBlogForm.$setPristine();
			vm.newBlogShortname = '';
			vm.successMessage.show();
			blogService.flushBlogs();
			threadService.flushThreads();
			blogService.getBlogs(true, true).then(function(blogs) {
				vm.blogs = blogs;
			});
		}

		function failure() {
			vm.failureMessage.show();
		}

		function createBlog() {
			if (!validateNewBlog()) {
				return;
			}
			var shortnameExists = _.some(vm.blogs, function(blog) {
				return blog.BlogShortname === vm.newBlogShortname;
			});
			if (shortnameExists) {
				vm.duplicateErrorNotification.show();
				return;
			}
			if (vm.newBlogShortname !== '') {
				blogService.createBlog(vm.newBlogShortname).then(success).catch(failure);
			}
		}

		function validateNewBlog() {
			if (!vm.newBlogForm.$valid) {
				if (vm.newBlogForm.newBlogShortname.$error.required) {
					vm.emptyBlogShortnameError.show();
				}
				if (!vm.newBlogForm.newBlogShortname.$error.required &&
					vm.newBlogForm.newBlogShortname.$error.pattern) {
					vm.invalidBlogShortnameError.show();
				}
				return false;
			}
			return true;
		}

		function untrackBlog(blog) {
			var message = 'This will untrack all thread(s) ';
			message += 'associated with ' + blog.BlogShortname + ' from your account. Continue?';
			var confirm = $mdDialog.confirm()
				.title('Untrack Thread(s)')
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
