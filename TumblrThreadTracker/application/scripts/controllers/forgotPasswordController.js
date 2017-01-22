'use strict';
(function() {
	angular.module('rpthreadtracker')
		.controller('ForgotPasswordController',
		[
			'$scope', '$controller', '$location', 'sessionService', 'TrackerNotification',
			'BodyClass', forgotPasswordController
		]);

	/** @this forgotPasswordController */
	// eslint-disable-next-line valid-jsdoc, max-params, max-len, max-statements
	function forgotPasswordController($scope, $controller, $location, sessionService, TrackerNotification, BodyClass) {
		var vm = this;
		angular.extend(vm, $controller('BaseController as base', {'$scope': $scope}));
		BodyClass.set('signin-page');
		vm.submitForgotPassword = submitForgotPassword;

		function submitForgotPassword() {
			vm.error = '';
			vm.success = '';
			vm.loading = true;
			sessionService.submitForgotPassword(vm.username).then(success, fail);
		}

		function success() {
			vm.loading = false;
			new TrackerNotification()
				.withMessage('Success. Check your email box for a temporary password.')
				.withType('success')
				.show();
		}

		function fail() {
			vm.loading = false;
			new TrackerNotification()
				.withMessage('Unknown error. Please try again later.')
				.withType('error')
				.show();
		}
	}
}());
