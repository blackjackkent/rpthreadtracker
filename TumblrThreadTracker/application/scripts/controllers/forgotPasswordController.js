(function() {
	'use strict';
	angular.module('rpthreadtracker')
		.controller('ForgotPasswordController',
		[
			'$scope', '$location', 'sessionService', 'TrackerNotification',
			forgotPasswordController
		]);

	function forgotPasswordController($scope, $location, sessionService, TrackerNotification) {
		$scope.setBodyClass('signin-page');
		$scope.submitForgotPassword = submitForgotPassword;

		function submitForgotPassword() {
			$scope.error = '';
			$scope.success = '';
			$scope.loading = true;
			sessionService.submitForgotPassword($scope.username).then(success, fail);
		}

		function success() {
			$scope.loading = false;
			new TrackerNotification()
				.withMessage('Success. Check your email box for a temporary password.')
				.withType('success')
				.show();
		}

		function fail() {
			$scope.loading = false;
			new TrackerNotification()
				.withMessage('Unknown error. Please try again later.')
				.withType('error')
				.show();
		}
	}
})();