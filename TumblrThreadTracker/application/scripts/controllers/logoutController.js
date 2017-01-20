(function() {
	"use strict";
	angular.module("rpthreadtracker")
		.controller("LogoutController",
		[
			"$scope", "$location", "blogService", "threadService", "sessionService",
			logoutController
		]);

	function logoutController($scope, $location, blogService, threadService, sessionService) {
		blogService.flushBlogs();
		threadService.flushThreads();
		$scope.user = null;
		$scope.userId = null;
		sessionService.logout();
		$location.path("/");
	}
})();