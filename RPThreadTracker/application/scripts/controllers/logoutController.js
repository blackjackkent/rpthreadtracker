'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('LogoutController',
		[
			'$scope', '$controller', '$location', 'blogService', 'threadService', 'sessionService',
			logoutController
		]);

	/** @this logoutController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len
	function logoutController($scope, $controller, $location, blogService, threadService, sessionService) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		logout();

		function logout() {
			blogService.flushBlogs();
			threadService.flushThreads();
			vm.user = null;
			vm.userId = null;
			sessionService.logout();
			$location.path('/');
		}
	}
}());
