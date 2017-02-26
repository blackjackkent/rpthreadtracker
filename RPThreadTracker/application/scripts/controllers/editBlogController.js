'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('EditBlogController',
		[
			'$scope', '$controller', '$routeParams', '$location', 'sessionService',
			'contextService', 'blogService', 'threadService', 'BodyClass',
			'notificationService', 'NOTIFICATION_TYPES', 'pageId',
			editBlogController
		]);

	/** @this editBlogController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function editBlogController($scope, $controller, $routeParams, $location, sessionService, contextService, blogService, threadService, BodyClass, notificationService, NOTIFICATION_TYPES, pageId) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		sessionService.loadUser(vm);
		BodyClass.set('');
		initView(parseInt($routeParams.userBlogId));

		function submitBlog() {
			if (!validateEditedBlog()) {
				return;
			}
			blogService.editBlog(vm.blogToEdit).then(success, failure);
		}

		function validateEditedBlog() {
			var shortnameExists = _.some(vm.allBlogs, function(blog) {
				return blog.BlogShortname === vm.blogToEdit.BlogShortname;
			});
			if (!vm.editBlogForm.$valid || shortnameExists) {
				var emptyShortname = vm.editBlogForm.newBlogShortname.$error.required;
				var invalidShortname = !vm.editBlogForm.newBlogShortname.$error.required &&
					vm.editBlogForm.newBlogShortname.$error.pattern;
				var errorData = {
					'newBlogShortname': vm.blogToEdit.BlogShortname,
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

		function initView(userBlogId) {
			vm.pageId = pageId;
			vm.submitBlog = submitBlog;
			vm.shortnameRegex = '[A-z|\\d|\\-]+';
			blogService.getBlogs(true, true).then(function(blogs) {
				vm.allBlogs = blogs;
				vm.blogToEdit = angular.copy(_.find(blogs, function(blog) {
					return blog.UserBlogId === userBlogId;
				}));
				vm.currentBlogShortname = angular.copy(vm.blogToEdit.BlogShortname);
			});
		}

		function success() {
			blogService.flushBlogs();
			var type = NOTIFICATION_TYPES.UPDATE_BLOGS_SUCCESS;
			notificationService.show(type);
			$location.path('/manage-blogs');
		}

		function failure() {
			vm.generalErrorMessage.show();
		}
	}
}());
